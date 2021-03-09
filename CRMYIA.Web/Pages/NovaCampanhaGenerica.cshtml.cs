using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CRMYIA.Business;
using System.Security.Claims;
using CRMYIA.Business.Util;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CRMYIA.Web.Pages
{
    public class NovaCampanhaGenerica : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }

        [BindProperty]
        public Campanha Entity { get; set; }
        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }

        #endregion

        #region Construtores
        public NovaCampanhaGenerica(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Campanha();
            else
                Entity = Business.CampanhaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());

            Entity.DataCadastro = DateTime.Now;
            CarregarLists();
            return Page();
        }

        public IActionResult OnPostListCampanha()
        {
            bool status = false;
           List<Campanha> ListCampanha =  Business.CampanhaModel.GetList();
            if(ListCampanha != null)
            {
                status = true;
            }
            return new JsonResult(new { status = status, retorno = ListCampanha });
        }


        //public IActionResult OnPostSalvar(/*[FromBody] Campanha dados*/)
        public async Task<IActionResult> OnPostSalvarAsync(IFormFile File, Campanha formData)
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            //dados.DataCadastro = DateTime.Now;
            bool status = false;
            try
            {
                var Files = Request.Form.Files.FirstOrDefault();

                string NomeArquivoOriginal = File.FileName;
                string NomeArquivo = string.Empty;

                if (Entity.IdCampanha == 0)
                {
                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var auxFile = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                    using (var fileStream = new FileStream(auxFile, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Entity.Url = Business.Util.Util.GetSlug(Entity.Descricao);
                    Entity.CaminhoArquivo = "ArquivoCampanha/";
                    Entity.NomeArquivo = NomeArquivo;
                    Business.CampanhaModel.Add(Entity);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    status = true;
                }
                else
                {
                    //string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                    //if ((System.IO.File.Exists(_imageToBeDeleted)))
                    //{
                    //    System.IO.File.Delete(_imageToBeDeleted);
                    //}

                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Entity.CaminhoArquivo = "ArquivoCampanha/";
                    Entity.NomeArquivo = NomeArquivo;
                    Business.CampanhaModel.Update(Entity);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados atualizado com sucesso!");
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }

        public IActionResult OnPostGetSubCategoria([FromBody] Campanha obj)
        {
            List<Campanha> Entity = null;
            bool status = false;
            if (obj.IdCampanhaReferencia.ToString() != null)
            {
                Entity = Business.CampanhaModel.GetSubCategoria(obj.IdCampanhaReferencia.ToString());
                if(Entity.Count() > 0)
                {
                    status = true;
                }
                
            }
            return new JsonResult(new { status = status, retorno = Entity });
            //return Page();
        }
        public void CarregarLists()
        {
            ListCampanha = Business.CampanhaModel.GetList();
        }

        #endregion
    }
}
