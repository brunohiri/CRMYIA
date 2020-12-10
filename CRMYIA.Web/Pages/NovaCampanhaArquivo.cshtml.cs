using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Business;
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
    public class NovaCampanhaArquivo : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }
        [BindProperty]
        public CampanhaArquivo Entity { get; set; }

        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }

        [BindProperty]
        public List<CampanhaArquivo> ListEntity { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        #endregion

        #region Construtores
        public NovaCampanhaArquivo(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
            {
                Entity = new CampanhaArquivo();
                Entity.DataCadastro = DateTime.Now;
            }
            else
            {
                Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ImagemDiferente = Entity.NomeArquivo;
            }

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
                    var msg = "";
                    if (Entity.IdCampanhaArquivo == 0)
                    {
                        msg = "Carregado";
                        string NomeArquivoOriginal = NomeArquivoCampanha.FileName;
                        string NomeArquivo = string.Empty;
                        NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                        var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await NomeArquivoCampanha.CopyToAsync(fileStream);
                        }
                        CampanhaArquivoModel.Add(new CampanhaArquivo()
                        {
                            IdCampanha = Entity.IdCampanha,
                            Descricao = Entity.Descricao,
                            CaminhoArquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"]),
                            NomeArquivo = NomeArquivo,
                            Observacao = Entity.Observacao,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                        });
                        CampanhaArquivo EntityArquivoLead = CampanhaArquivoModel.GetLastId();
                        Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Sucesso, string.Format("Arquivo de Campanha carregado com sucesso!"));
                    }
                    else
                    {
                        msg = "Alterado";
                        string NomeArquivo = string.Empty;
                        if (!string.IsNullOrEmpty(ImagemDiferente) && NomeArquivoCampanha != null)
                        {
                            string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], ImagemDiferente);
                            if ((System.IO.File.Exists(_imageToBeDeleted)))
                            {
                                System.IO.File.Delete(_imageToBeDeleted);
                            } 
                            string NomeArquivoOriginal = NomeArquivoCampanha.FileName;
                           
                            NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                            var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                            using (var fileStream = new FileStream(file, FileMode.Create))
                            {
                                await NomeArquivoCampanha.CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            NomeArquivo = ImagemDiferente;
                        }
                       
                        CampanhaArquivoModel.Update(new CampanhaArquivo()
                        {
                            IdCampanhaArquivo = Entity.IdCampanhaArquivo,
                            IdCampanha = Entity.IdCampanha,
                            Descricao = Entity.Descricao,
                            CaminhoArquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"]),
                            NomeArquivo = NomeArquivo,
                            Observacao = Entity.Observacao,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                        });
                        CampanhaArquivo EntityArquivoLead = CampanhaArquivoModel.GetLastId();
                        Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Sucesso, string.Format("Arquivo de Campanha " + msg + " com sucesso!"));
                    }
                }
            }

            CarregarLists();
            return Page();
        }

        public void CarregarLists()
        {
            ListEntity = CampanhaArquivoModel.GetList();
            ListCampanha = CampanhaModel.GetList();
        }

        #endregion
    }
}
