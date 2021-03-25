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
    public class BannersDeOperadora : PageModel
    {
        public List<BannerOperadoraViewModel> ListBanner { get; set; }
        public List<Operadora> ListOperadora { get; set; }
        [BindProperty]
        public UsuarioCorretorViewModel UsuarioEntity { get; set; }
        public IActionResult OnGet(string Id)
        {
            //long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            //UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
            //ListOperadora = OperadoraModel.GetList();
            //ListBannerOperadora = BannerOperadoraModel.GetList();

            if (Id.IsNullOrEmpty())
            {
                //
            }
            else
            {
                List<BannerOperadora> EntityBannerOperadora = null;
                List<long> IdBanner = new List<long>();
                EntityBannerOperadora = BannerOperadoraModel.GetAllBannerOperadora(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                foreach (var Item in EntityBannerOperadora)
                {
                    IdBanner.Add(Item.IdBanner.ToString().ExtractLong());
                }
                UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
                ListBanner = BannerOperadoraModel.GetAllBanner(IdBanner);
                ListOperadora = OperadoraModel.GetList();
            }

            return Page();
        }
    }
}
