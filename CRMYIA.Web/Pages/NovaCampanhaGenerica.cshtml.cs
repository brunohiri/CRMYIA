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
        public List<GrupoCorretor> ListGrupoCorretor { get; set; }
        [BindProperty]
        public string NomeDoArquivo {get; set;}
        [BindProperty]
        public List<GrupoCorretorCampanha> ListGrupoCorretorCampanha { get; set; }

        [BindProperty]
        public Campanha Entity { get; set; }
        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }
        [BindProperty]
        public List<Calendario> ListCalendario { get; set; }

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
            {
                Entity = new Campanha();
            }
            else
            {
                int i = 0;
                List<GrupoCorretorCampanha> EntityGrupoCorretorCampanha = null;
                Entity = Business.CampanhaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListGrupoCorretorCampanha = GrupoCorretorCampanhaModel.Get(Entity.IdCampanha);
            }

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
            string[] IdGrupoCorretores = Request.Form["IdGrupoCorretor"];
            bool gravado = false;


            ListGrupoCorretorCampanha = GrupoCorretorCampanhaModel.Get(Entity.IdCampanha);
          
           
            bool status = false;
            try
            {
                var Files = Request.Form.Files.FirstOrDefault();

                string NomeArquivoOriginal = File.FileName;
                string NomeArquivo = string.Empty;

                if (Entity.IdCampanha == 0 && NomeArquivoOriginal != null && IdGrupoCorretores != null)
                {
                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var auxFile = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                    using (var fileStream = new FileStream(auxFile, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Entity.CaminhoArquivo = "ArquivoCampanha/";
                    Entity.NomeArquivo = NomeArquivo;
                    Entity.QuantidadeDownload = 0;
                    Business.CampanhaModel.Add(Entity);

                    foreach (var Item in IdGrupoCorretores) {
                        if(Item != null)
                            Business.GrupoCorretorCampanhaModel.Add(new GrupoCorretorCampanha() {
                                IdGrupoCorretor = (byte)Convert.ToInt32(Item),
                                IdCampanha = Entity.IdCampanha
                            }); 
                    }

                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    status = true;
                }
                else if (NomeArquivoOriginal != null && Entity.NomeArquivo != null)
                {
                    string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], Entity.NomeArquivo);
                    if ((System.IO.File.Exists(_imageToBeDeleted)))
                    {
                        System.IO.File.Delete(_imageToBeDeleted);
                    }

                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var auxFile = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                    using (var fileStream = new FileStream(auxFile, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Entity.CaminhoArquivo = "ArquivoCampanha/";
                    Entity.NomeArquivo = NomeArquivo;
                    Business.CampanhaModel.Update(Entity);


                    List<GrupoCorretorCampanha> ListEntityGrupoCorretorCampanha = null;
                    ListEntityGrupoCorretorCampanha = GrupoCorretorCampanhaModel.Get(Entity.IdCampanha);

                    for(var i = 0; i<IdGrupoCorretores.Length; i++)
                    {
                        Business.GrupoCorretorCampanhaModel.Add(new GrupoCorretorCampanha()
                        {
                            IdGrupoCorretor = (byte)Convert.ToInt32(IdGrupoCorretores[i]),
                            IdCampanha = Entity.IdCampanha
                        });
                        if (i >= IdGrupoCorretores.Length - 1)
                        {
                            gravado = true;
                        }
                    }

                    if (gravado)
                    {
                        foreach (var Item in ListEntityGrupoCorretorCampanha)
                        {
                            Business.GrupoCorretorCampanhaModel.Delete(Item);
                        }
                    }

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

        //public IActionResult OnPostGetSubCategoria([FromBody] Campanha obj)
        //{
        //    List<Campanha> Entity = null;
        //    bool status = false;
        //    if (obj.IdCampanhaReferencia.ToString() != null)
        //    {
        //        Entity = Business.CampanhaModel.GetSubCategoria(obj.IdCampanhaReferencia.ToString());
        //        if(Entity.Count() > 0)
        //        {
        //            status = true;
        //        }
                
        //    }
        //    return new JsonResult(new { status = status, retorno = Entity });
        //    //return Page();
        //}
        public void CarregarLists()
        {
            ListCampanha = Business.CampanhaModel.GetList();
            ListCalendario = Business.CalendarioModel.GetList();
            ListGrupoCorretor = Business.GrupoCorretorModel.GetList();
        }

        #endregion
    }
}
