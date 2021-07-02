using CRMYIA.Business.ShiftData;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYIA.Business.YNDICA
{
    public class FilaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Fila Get(long IdFila)
        {
            Fila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.FilaItem)
                        .ThenInclude(z => z.IdFornecedorConsultaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdFila == IdFila)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Fila GetLastId()
        {
            Fila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Fila
                        .ToList()
                        .LastOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Fila> GetList()
        {
            List<Fila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.IdFornecedorNavigation)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataEntrada).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Fila> GetListByStatusFila(short IdStatusFila)
        {
            List<Fila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.FilaItem)
                        .ThenInclude(z => z.IdFornecedorConsultaNavigation)
                        .Where(x => x.IdStatusFila == IdStatusFila)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataEntrada)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Fila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Fila.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Fila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Fila.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Outros Métodos
        public static void ProcessarFila(Fila ItemFila)
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
                ItemFila.DataProcessamento = DateTime.Now;
                ItemFila.IdStatusFila = (byte)(EnumeradorModel.StatusFila.Processando);
                FilaModel.Update(ItemFila);
                #endregion

                Engine = new ShiftDataEngine();
                Parallel.ForEach(ItemFila.FilaItem, new ParallelOptions { MaxDegreeOfParallelism = QtdThreads }, itemParallel =>
                {
                    AccessToken = Engine.Login(out Mensagem);
                    if (!AccessToken.IsNullOrEmpty())
                    {
                        if (Util.Util.IsCnpj(itemParallel.Documento.PadronizarCNPJ()))
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
                FilaItemModel.GetQuantidadeProcessado(ItemFila.IdFila);
                ItemFila.IdStatusFila = (byte)(EnumeradorModel.StatusFila.Processado);
                FilaModel.Update(ItemFila);
                #endregion

            }
            catch (Exception)
            {
            }

        }
        #endregion
    }
}
