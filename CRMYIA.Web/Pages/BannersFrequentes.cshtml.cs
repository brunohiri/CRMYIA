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
    public class BannersFrequentes : PageModel
    {
        [BindProperty]
        public List<Campanha> ListaCampanhaArquivo { get; set; }//CapaViewModel
        [BindProperty]
        public UsuarioCorretorViewModel UsuarioEntity { get; set; }
        public IActionResult OnGet(string Id)
        {
            if (Id.IsNullOrEmpty())
            {
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
                Usuario EntityUsuario = null;
                EntityUsuario = UsuarioModel.Get(IdUsuario);
                ListaCampanhaArquivo = CampanhaModel.RankingDeCampanhaMaisBaixadas();
                //ListCapa = CapaModel.GetListaCapa(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong(), (byte)EntityUsuario.IdGrupoCorretor);
            }
            else
            {
               
            }
               
            return Page();
        }
    }
}
