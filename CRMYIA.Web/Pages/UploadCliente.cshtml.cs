using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class UploadClienteModel : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoLead { get; set; }

        [BindProperty]
        public List<ArquivoLead> ListEntity { get; set; }

        #endregion

        #region Construtores
        public UploadClienteModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Campos Incorretos!");
            }
            else
            {
                if (Mensagem == null)
                {
                    string NomeArquivoOriginal = NomeArquivoLead.FileName;
                    string NomeArquivo = string.Empty;

                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoLead"], NomeArquivo);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await NomeArquivoLead.CopyToAsync(fileStream);
                    }

                    ArquivoLeadModel.Add(new ArquivoLead()
                    {
                        CaminhoArquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoLead"]),
                        NomeArquivoOriginal = NomeArquivoOriginal,
                        NomeArquivoTratado = NomeArquivo,
                        QtdRegistros = 0,
                        DataCadastro = DateTime.Now
                    });

                    ArquivoLead EntityArquivoLead = ArquivoLeadModel.GetLastId();

                    int QtdRegistros = ImportarArquivo(file, EntityArquivoLead.IdArquivoLead);

                    EntityArquivoLead.QtdRegistros = QtdRegistros;
                    ArquivoLeadModel.Update(EntityArquivoLead);

                    Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Sucesso, string.Format("Arquivo de Lead, com {0:#,0} registros, importado com sucesso!", Convert.ToInt64(QtdRegistros)));
                }
            }

            CarregarLists();
            return Page();
        }

        public void CarregarLists()
        {
            ListEntity = ArquivoLeadModel.GetList();
        }

        public int ImportarArquivo(string ArquivoCompleto, long IdArquivoLead)
        {
            int QtdRegistros = 0;
            if (System.IO.File.Exists(ArquivoCompleto))
            {
                string[] Linhas = System.IO.File.ReadAllLines(ArquivoCompleto);

                List<TipoLead> ListTipoLead = TipoLeadModel.GetListIdDescricao();

                foreach (var Item in Linhas)
                {
                    if (QtdRegistros > 0)
                    {
                        ClienteModel.Add(new Cliente()
                        {
                            IdArquivoLead = IdArquivoLead,
                            Nome = Item.Split(';')[2].ToString(),
                            Idade = Item.Split(';')[8].ToString().ExtractInt32OrNull(),
                            Observacao = Item.Split(';')[0].ToString(),
                            IdTipoLead = ListTipoLead.Where(x => x.Descricao.RemoverAcentuacao().Contains(Item.Split(';')[1])).FirstOrDefault().IdTipoLead,
                            IdOrigem = OrigemLeadModel.GetByCode(Item.Split(';')[7].ToString().ExtractLong()).IdOrigem,
                            IdCidade = CidadeModel.GetByDescricao(Item.Split(';')[6].ToString()).IdCidade,
                            DataCadastro = DateTime.Now,
                            Ativo = true
                        });

                        Cliente EntityCliente = ClienteModel.GetLastId();

                        #region FonePrincipal
                        if (!Item.Split(';')[3].IsNullOrEmpty())
                            EmailModel.Add(new Email()
                            {
                                IdCliente = EntityCliente.IdCliente,
                                EmailConta = Item.Split(';')[3].ToString(),
                                DataCadastro = DateTime.Now,
                                Ativo = true
                            });
                        #endregion

                        #region FoneCelular
                        if (!Item.Split(';')[4].IsNullOrEmpty())
                            TelefoneModel.Add(new Telefone()
                            {
                                IdCliente = EntityCliente.IdCliente,
                                DDD = Item.Split(';')[4].ToString().Substring(0, 2),
                                Telefone1 = Item.Split(';')[4].ToString().Substring(2, Item.Split(';')[4].ToString().Length - 2),
                                WhatsApp = (Item.Split(';')[4].ToString().Length == 11),
                                DataCadastro = DateTime.Now,
                                Ativo = true
                            });
                        #endregion

                        #region Email
                        if (!Item.Split(';')[5].IsNullOrEmpty())
                            TelefoneModel.Add(new Telefone()
                            {
                                IdCliente = EntityCliente.IdCliente,
                                DDD = Item.Split(';')[5].ToString().Substring(0, 2),
                                Telefone1 = Item.Split(';')[5].ToString().Substring(2, Item.Split(';')[5].ToString().Length - 2),
                                WhatsApp = (Item.Split(';')[5].ToString().Length == 11),
                                DataCadastro = DateTime.Now,
                                Ativo = true
                            });
                        #endregion
                    }
                    QtdRegistros++;
                }
            }
            return QtdRegistros;
        }

        #endregion
    }
}
