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
    public class OperadoraSeguradora : PageModel
    {
        [BindProperty]
        public List<BannerOperadoraViewModel> ListBannerOperadora { get; set; }
        [BindProperty]
        public List<Operadora> ListOperadora { get; set; }
        [BindProperty]
        public UsuarioCorretorViewModel UsuarioEntity { get; set; }
        public IActionResult OnGet()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
            ListOperadora = OperadoraModel.GetList();
            ListBannerOperadora = BannerOperadoraModel.GetList();
            
            return Page();
        }
        //public IActionResult OnOperadoraPost(string param)
        //{
        //   var url = Url.Page("OperadoraSeguradora/BannersOperadoras", new { param = param});
        //    return Content(url);
        //}
    }
}
