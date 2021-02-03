using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class ListKPIModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public List<KPIMetaVida> ListKPIMetaVida { get; set; }
        [BindProperty]
        public List<KPIMetaValor> ListKPIMetaValor { get; set; }
        [BindProperty]
        public List<Perfil> ListPerfil { get; set; }
        [BindProperty]
        public int? UsuarioIdPerfil { get; set; }


        #endregion

        #region Construtores
        public ListKPIModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            CarregarLists();
            return Page();
        }
        public void OnPost()
        {
            byte id = byte.Parse(Request.Form["UsuarioIdPerfil"]);

            // lista de usuario apartir do perfil
            var users = UsuarioPerfilModel.GetList(id);
            CarregarLists();
        }
        #endregion

        public void CarregarLists()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            
            ListKPIMetaVida = KPIMetaVidaModel.GetList();
            ListKPIMetaValor = KPIMetaValorModel.GetList();
            ListPerfil = PerfilModel.GetList();
        }

    }
}