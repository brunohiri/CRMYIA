using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class MaterialDivulgacaoModel : PageModel
    {
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        #region Construtores
        public MaterialDivulgacaoModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        //[BindProperty(SupportsGet = true)]
        //public string Title { get; set; }

        //public string PublishUrl { get; set; }

        [BindProperty]
        public List<CampanhaArquivo> ListCampanhaArquivo { get; set; }
        public List<Campanha> ListCampanha { get; set; }
        public void OnGet(string Id = null)
        {
            //PublishUrl = Title;
            if (Id.IsNullOrEmpty())
                ListsCampanhaId();
            else
                ListsCampanhaId(Id);
            
        }

        public IActionResult OnGetListarCampanha()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            int i;

            long IdCampanhaArquivo = 0;
            long? IdCampanha = 0;
            string Descricao = "";
            string CaminhoArquivo = "";
            string NomeArquivo = "";
            string Width = "";
            string Height = "";
            string RedesSociais = "";
            string TipoPostagem = "";
            DateTime DataCadastro = DateTime.MinValue;
            bool Ativo = false;
            Campanha IdCampanhaNavigation = new Campanha();

            List<CampanhaArquivoViewModel> AuxCampanhaArquivo = new List<CampanhaArquivoViewModel>();
            List<CampanhaArquivo> CampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            List<Campanha> Campanha = CampanhaModel.GetListOrderById();
            UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);

            foreach (var ItemCampanha in Campanha)
            {
                i = 0;
                foreach (var ItemCampanhaArquivo in CampanhaArquivo)
                {
                    if (ItemCampanha.IdCampanha == ItemCampanhaArquivo.IdCampanha)
                    {

                        if (i + 1 < CampanhaArquivo.Count)
                        {
                            NomeArquivo += ItemCampanhaArquivo.NomeArquivo + "|";
                            Width += ItemCampanhaArquivo.Width + "|";
                            Height += ItemCampanhaArquivo.Height + "|";
                        }
                        else
                        {
                            NomeArquivo += ItemCampanhaArquivo.NomeArquivo;
                            Width += ItemCampanhaArquivo.Width;
                            Height += ItemCampanhaArquivo.Height;
                          
                        }
                        IdCampanhaArquivo = ItemCampanhaArquivo.IdCampanhaArquivo;
                        IdCampanha = ItemCampanhaArquivo.IdCampanha;
                        Descricao = ItemCampanhaArquivo.IdInformacaoNavigation.Descricao;
                        CaminhoArquivo = ItemCampanhaArquivo.CaminhoArquivo;
                        RedesSociais = ItemCampanhaArquivo.RedesSociais;
                        TipoPostagem = ItemCampanhaArquivo.TipoPostagem;
                        DataCadastro = ItemCampanhaArquivo.DataCadastro;
                        Ativo = ItemCampanhaArquivo.Ativo;
                        IdCampanhaNavigation = ItemCampanhaArquivo.IdCampanhaNavigation;

                    }
                    i++;
                }

                AuxCampanhaArquivo.Add(new CampanhaArquivoViewModel()
                {
                    IdCampanhaArquivo = IdCampanhaArquivo,
                    IdCampanha = IdCampanha,
                    Descricao = Descricao,
                    CaminhoArquivo = CaminhoArquivo,
                    NomeArquivo = NomeArquivo,
                    Width = Width,
                    Height = Height,
                    RedesSociais = RedesSociais,
                    TipoPostagem = TipoPostagem,
                    DataCadastro = DataCadastro,
                    Ativo = Ativo,
                    IdCampanhaNavigation = IdCampanhaNavigation
                });
                NomeArquivo = "";
                Width = "";
                Height = "";

            }  
            if (AuxCampanhaArquivo != null && Campanha != null)
            {
                status = true;
            }
            return new JsonResult(new { status = status, campanhaArquivo = AuxCampanhaArquivo, campanha = Campanha, usuarioEntity = UsuarioEntity });
        }

        private void ListsCampanhaId(string Id = null)
        {
            ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
        }

        private void ListsCampanhaId()
        {
            ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            ListCampanha = CampanhaModel.GetListOrderById();
        }
    }
}
