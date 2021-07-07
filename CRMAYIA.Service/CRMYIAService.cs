using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CRMYIA.Data;
using CRMYIA.Business;
using CRMYIA.Data.Entities;
using CRMYIA.Business.YNDICA;
using CRMYIA.Business.Util;
using System.IO;
using CRMYIA.Business.ShiftData;
using Newtonsoft.Json;

namespace CRMYIA.Service
{
    internal class CRMYIAService : ServiceBase
    {
        private System.Timers.Timer timer;
        private IConfiguration configuration;

        public CRMYIAService(IConfiguration _configuration)
        {
            ServiceName = "CRMYIAService";
            configuration = _configuration;
        }

        private void InitializeTimer()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.AutoReset = true;
                timer.Interval = 60000; //1 minuto
                //timer.Interval = 600000; //10 minutos
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }
        }

        protected override void OnStart(string[] args)
        {
            InitializeTimer();
            timer.Start();
        }

        protected override void OnStop()
        {

        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();


            try
            {
                Console.WriteLine("Iniciando Serviço! " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                try
                {
                    ExecutarServico();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("Finalizando Serviço! " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
            }
            finally
            {
                timer.Enabled = true;
                timer.Start();
            }
        }

        #region Serviço
        public void ExecutarServico()
        {

            try
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: ExecutarServico\nInício ExecutarServico\n");

                #region Verificar Fila para Processamento do Enriquecimento do YNDICA
                List<Fila> ListEntityFila = null;

                ListEntityFila = FilaModel.GetListByStatusFila((short)(EnumeradorModel.StatusFila.Aguardando));

                if ((ListEntityFila != null) && (ListEntityFila.Count() > 0))
                {
                    ListEntityFila.ToList().ForEach(delegate (Fila ItemFila)
                    {
                        ProcessarFila(ItemFila.IdFila);

                        #region GerarArquivo
                        Fila UpdateFila = FilaModel.GetUpdate(ItemFila.IdFila);
                        UpdateFila.IdStatusFila = (byte?)(EnumeradorModel.StatusFila.Compactando);
                        FilaModel.Update(UpdateFila);

                        var path = Path.Combine(configuration["YndicaProcessado"], ItemFila.NomeArquivoSaida);
                        List<LayoutPJ> ListLayoutPJ = LayoutPJModel.GetByIdFila(ItemFila.IdFila);
                        StringBuilder sbFile = new StringBuilder();
                        sbFile.AppendLine("RazaoSocial;CNPJ;Endereco;CEP;Cidade;UF;CNAE;Telefone1;Telefone2;Telefone3;Telefone4;Email1;Email2;SocioNome;SocioDataNascimento;SocioNomeMae;SocioEndereco;SocioCEP;SocioCidade;SocioUF;SocioTelefone1;SocioTelefone2;SocioTelefone3;SocioTelefone4;SocioEmail1;SocioEmail2;SocioPerfilConsumo");
                        if (ListLayoutPJ != null && ListLayoutPJ.Count() > 0)
                        {
                            ListLayoutPJ.OrderBy(x=>x.RazaoSocial).ToList().ForEach(delegate (LayoutPJ ItemLayoutPJ)
                            {
                                sbFile.AppendLine(ItemLayoutPJ.RazaoSocial + ";" + ItemLayoutPJ.CNPJ + ";" + ItemLayoutPJ.Endereco + ";" + ItemLayoutPJ.CEP + ";" + ItemLayoutPJ.Cidade + ";" + ItemLayoutPJ.UF + ";" + ItemLayoutPJ.CNAE + ";" + ItemLayoutPJ.Telefone1 + ";" + ItemLayoutPJ.Telefone2 + ";" + ItemLayoutPJ.Telefone3 + ";" + ItemLayoutPJ.Telefone4 + ";" + ItemLayoutPJ.Email1 + ";" + ItemLayoutPJ.Email2 + ";" + ItemLayoutPJ.SocioNome + ";" + ItemLayoutPJ.SocioDataNascimento + ";" + ItemLayoutPJ.SocioNomeMae + ";" + ItemLayoutPJ.SocioEndereco + ";" + ItemLayoutPJ.SocioCEP + ";" + ItemLayoutPJ.SocioCidade + ";" + ItemLayoutPJ.SocioUF + ";" + ItemLayoutPJ.SocioTelefone1 + ";" + ItemLayoutPJ.SocioTelefone2 + ";" + ItemLayoutPJ.SocioTelefone3 + ";" + ItemLayoutPJ.SocioTelefone4 + ";" + ItemLayoutPJ.SocioEmail1 + ";" + ItemLayoutPJ.SocioEmail2 + ";" + ItemLayoutPJ.SocioPerfilConsumo);
                            });
                        }
                        System.IO.File.WriteAllText(path, sbFile.ToString());

                        UpdateFila.IdStatusFila = (byte?)(EnumeradorModel.StatusFila.Exportado);
                        FilaModel.Update(UpdateFila);
                        #endregion
                    });
                }

                #endregion
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: ExecutarServico\n" + ex.Message + "\n");
            }

        }

        public void ProcessarFila(long IdFila)
        {
            ShiftDataEngine Engine = null;
            string AccessToken = string.Empty;
            string Mensagem = string.Empty;
            int QtdThreads = 0;
            try
            {
#if (DEBUG)
                QtdThreads = 1;
#endif
#if (!DEBUG)
                QtdThreads = 10;
#endif

                #region Atualizar StatusFila para Processando
                Fila ItemFila = FilaModel.GetUpdate(IdFila);
                ItemFila.DataProcessamento = DateTime.Now;
                ItemFila.IdStatusFila = (byte?)(EnumeradorModel.StatusFila.Processando);
                FilaModel.Update(ItemFila);
                #endregion

                List<FilaItem> ListFilaItem = FilaItemModel.GetList(IdFila);

                Engine = new ShiftDataEngine();
                Parallel.ForEach(ListFilaItem, new ParallelOptions { MaxDegreeOfParallelism = QtdThreads }, itemParallel =>
                {
                    if (AccessToken.IsNullOrEmpty())
                    {
                        AccessToken = Engine.Login(out Mensagem);
                        if (!Mensagem.IsNullOrEmpty())
                        {
                            System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: ProcessarFila\nMensagem API:" + Mensagem + "\n");
                        }
                    }
                    if (!AccessToken.IsNullOrEmpty())
                    {
                        if (Util.IsCnpj(itemParallel.Documento.PadronizarCNPJ()))
                        {
                            ShiftDataResultPessoaJuridica EntityPJ = null;
                            EntityPJ = Engine.ExecutePessoaJuridica(AccessToken, itemParallel.Documento.PadronizarCNPJ(), out Mensagem);

                            #region Salvar na FornecedorConsulta
                            FornecedorConsulta EntityFornecedorConsulta = new FornecedorConsulta()
                            {
                                DataConsulta = DateTime.Now,
                                Documento = itemParallel.Documento.PadronizarCNPJ(),
                                IdFornecedor = (byte)(EnumeradorModel.Fornecedor.ShiftData),
                                Metodo = "PessoaJuridica",
                                IdUsuario = ItemFila.IdUsuario,
                                RetornoJson = JsonConvert.SerializeObject(EntityPJ),
                                IP = ItemFila.IP
                            };
                            long IdFornecedorConsulta = FornecedorConsultaModel.Add(EntityFornecedorConsulta);
                            #endregion

                            if (Mensagem.IsNullOrEmpty())
                            {
                                #region Adicionar no LayoutPJ

                                LayoutPJ EntityLayoutPJ = GetPJ(EntityPJ, itemParallel.IdFilaItem);

                                if (EntityPJ.Socios != null && EntityPJ.Socios.Count() > 0)
                                {
                                    EntityPJ.Socios.ToList().ForEach(delegate (ShiftDataResultSocio ItemSocio)
                                    {
                                        if (!ItemSocio.CPF.IsNullOrEmpty())
                                        {
                                            ShiftDataResultPessoaFisica EntityPF = Engine.ExecutePessoaFisica(AccessToken, ItemSocio.CPF?.PadronizarCPF(), out Mensagem);

                                            #region Salvar na FornecedorConsulta
                                            FornecedorConsulta EntityFornecedorConsulta = new FornecedorConsulta()
                                            {
                                                DataConsulta = DateTime.Now,
                                                Documento = ItemSocio.CPF?.PadronizarCPF(),
                                                IdFornecedor = (byte)(EnumeradorModel.Fornecedor.ShiftData),
                                                Metodo = "PessoaFisica",
                                                IdUsuario = ItemFila.IdUsuario,
                                                RetornoJson = JsonConvert.SerializeObject(EntityPF),
                                                IP = ItemFila.IP
                                            };
                                            FornecedorConsultaModel.Add(EntityFornecedorConsulta);

                                            if (Mensagem.IsNullOrEmpty())
                                            {
                                                EntityLayoutPJ.SocioNome = EntityPF.Nome;
                                                EntityLayoutPJ.SocioDataNascimento = EntityPF.DataNascimento?.ToString("dd/MM/yyyy");
                                                EntityLayoutPJ.SocioNomeMae = EntityPF.NomeMae;
                                                EntityLayoutPJ.SocioEndereco = (EntityPF.Enderecos != null && EntityPF.Enderecos.Count() > 0 ? EntityPF.Enderecos.FirstOrDefault().EnderecoCompleto : null);
                                                EntityLayoutPJ.SocioCEP = (EntityPF.Enderecos != null && EntityPF.Enderecos.Count() > 0 ? EntityPF.Enderecos.FirstOrDefault().CEP : null);
                                                EntityLayoutPJ.SocioCidade = (EntityPF.Enderecos != null && EntityPF.Enderecos.Count() > 0 ? EntityPF.Enderecos.FirstOrDefault().Cidade : null);
                                                EntityLayoutPJ.SocioUF = (EntityPF.Enderecos != null && EntityPF.Enderecos.Count() > 0 ? EntityPF.Enderecos.FirstOrDefault().UF : null);

                                                if (EntityPF.Telefones != null && EntityPF.Telefones.Count() > 0)
                                                {
                                                    EntityPF.Telefones.ToList().ForEach(delegate (Business.ShiftData.ShiftDataResultTelefone Item)
                                                    {
                                                        if (Item.Ranking == 1)
                                                            EntityLayoutPJ.SocioTelefone1 = Item.DDD.ToString() + Item.Telefone.ToString();
                                                        else
                                                        if (Item.Ranking == 2)
                                                            EntityLayoutPJ.SocioTelefone2 = Item.DDD.ToString() + Item.Telefone.ToString();
                                                        else
                                                        if (Item.Ranking == 3)
                                                            EntityLayoutPJ.SocioTelefone3 = Item.DDD.ToString() + Item.Telefone.ToString();
                                                        else
                                                        if (Item.Ranking == 4)
                                                            EntityLayoutPJ.SocioTelefone4 = Item.DDD.ToString() + Item.Telefone.ToString();
                                                    });
                                                }
                                                #endregion

                                                #region Emails
                                                if (EntityPF.Emails != null && EntityPF.Emails.Count() > 0)
                                                {
                                                    EntityPF.Emails.ToList().ForEach(delegate (Business.ShiftData.ShiftDataResultEmail Item)
                                                    {
                                                        if (Item.Ranking == 1)
                                                            EntityLayoutPJ.SocioEmail1 = Item.Email;
                                                        else
                                                        if (Item.Ranking == 2)
                                                            EntityLayoutPJ.SocioEmail2 = Item.Email;
                                                    });
                                                }
                                                #endregion
                                            }
                                        }
                                        LayoutPJModel.Add(EntityLayoutPJ);

                                        EntityLayoutPJ = GetPJ(EntityPJ, itemParallel.IdFilaItem);
                                    });
                                }
                                else
                                    LayoutPJModel.Add(EntityLayoutPJ);

                                #endregion
                            }
                            else
                                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: ProcessarFila\nMensagem API:" + Mensagem + "\n");

                            #region Atualizar FilaItem
                            itemParallel.Processado = true;
                            itemParallel.IdFornecedorConsulta = IdFornecedorConsulta;
                            FilaItemModel.Update(itemParallel);
                            #endregion
                        }
                    }
                });

                #region Atualizar StatusFila para Processado
                ItemFila.DataSaida = DateTime.Now;
                ItemFila.QtdProcessado = FilaItemModel.GetQuantidadeProcessado(ItemFila.IdFila);
                ItemFila.QtdSaida = FilaItemModel.GetQuantidadeProcessado(ItemFila.IdFila);
                ItemFila.IdStatusFila = (byte)(EnumeradorModel.StatusFila.Processado);
                FilaModel.Update(ItemFila);
                #endregion

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: ProcessarFila\n" + ex.Message + "\n");
            }

        }

        private static LayoutPJ GetPJ(ShiftDataResultPessoaJuridica EntityPJ, long IdFilaItem)
        {
            try
            {

                LayoutPJ EntityLayoutPJ = new LayoutPJ()
                {
                    IdFilaItem = IdFilaItem,
                    RazaoSocial = EntityPJ.NomeRazao,
                    CNPJ = EntityPJ.CNPJ,
                    CNAE = (EntityPJ.CodigoCnae.IsNullOrEmpty() ? string.Empty : EntityPJ.CodigoCnae + " - " + EntityPJ.DescricaoCnae),
                    Endereco = (EntityPJ.Enderecos != null && EntityPJ.Enderecos.Count() > 0 ? EntityPJ.Enderecos.FirstOrDefault().EnderecoCompleto : null),
                    CEP = (EntityPJ.Enderecos != null && EntityPJ.Enderecos.Count() > 0 ? EntityPJ.Enderecos.FirstOrDefault().CEP : null),
                    Cidade = (EntityPJ.Enderecos != null && EntityPJ.Enderecos.Count() > 0 ? EntityPJ.Enderecos.FirstOrDefault().Cidade : null),
                    UF = (EntityPJ.Enderecos != null && EntityPJ.Enderecos.Count() > 0 ? EntityPJ.Enderecos.FirstOrDefault().UF : null)
                };

                #region Telefones
                if (EntityPJ.Telefones != null && EntityPJ.Telefones.Count() > 0)
                {
                    EntityPJ.Telefones.ToList().ForEach(delegate (Business.ShiftData.ShiftDataResultTelefone Item)
                    {
                        if (Item.Ranking == 1)
                            EntityLayoutPJ.Telefone1 = Item.DDD.ToString() + Item.Telefone.ToString();
                        else
                        if (Item.Ranking == 2)
                            EntityLayoutPJ.Telefone2 = Item.DDD.ToString() + Item.Telefone.ToString();
                        else
                        if (Item.Ranking == 3)
                            EntityLayoutPJ.Telefone3 = Item.DDD.ToString() + Item.Telefone.ToString();
                        else
                        if (Item.Ranking == 4)
                            EntityLayoutPJ.Telefone4 = Item.DDD.ToString() + Item.Telefone.ToString();
                    });
                }
                #endregion

                #region Emails
                if (EntityPJ.Emails != null && EntityPJ.Emails.Count() > 0)
                {
                    EntityPJ.Emails.ToList().ForEach(delegate (Business.ShiftData.ShiftDataResultEmail Item)
                    {
                        if (Item.Ranking == 1)
                            EntityLayoutPJ.Email1 = Item.Email;
                        else
                        if (Item.Ranking == 2)
                            EntityLayoutPJ.Email2 = Item.Email;
                    });
                }
                #endregion

                return EntityLayoutPJ;

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: GetPJ\n" + ex.Message + "\n");
                return null;
            }
        }
        #endregion

    }
}
