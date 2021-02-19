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
        public void OnGet(string Id = null, string url = null)
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
            List<CampanhaArquivo> CampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            List<Campanha> Campanha = CampanhaModel.GetListOrderById();
            //Usuario UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
            if (CampanhaArquivo != null && Campanha != null)
            {
                status = true;
            }
            return new JsonResult(new { status = status, campanhaArquivo = CampanhaArquivo, campanha = Campanha, /*usuarioEntity = UsuarioEntity*/ });
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
