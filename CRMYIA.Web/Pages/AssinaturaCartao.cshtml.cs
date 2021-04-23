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
    public class AssinaturasCartaos : PageModel
    {
        [BindProperty]
        public List<AssinaturaCartaoViewModel> ListAssinaturaCartao { get; set; }
        [BindProperty]
        public UsuarioCorretorViewModel UsuarioEntity { get; set; }
        public IActionResult OnGet(string Id)
        {
            if (Id.IsNullOrEmpty()) 
            {
            } 
            else
            {
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                Usuario UsuarioCorretor = UsuarioModel.Get(IdUsuario);
                UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
                ListAssinaturaCartao = AssinaturaCartaoModel.GetListaAssinatura(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong(), (byte)UsuarioCorretor.IdGrupoCorretor);
            }
          
            return Page();
        }
    }
}
