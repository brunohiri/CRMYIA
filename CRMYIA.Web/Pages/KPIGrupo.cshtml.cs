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
    public class KPIGrupoModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public KPIGrupo KPIGrupoEntity { get; set; }
        [BindProperty]
        public List<ListaCorretorViewModel> ListCargo { get; set; }
        [BindProperty]
        public int? IdPerfil { get; set; }
        #endregion

        #region Construtores
        public KPIGrupoModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            CarregarLists(1);
            KPIGrupoEntity = new KPIGrupo();
            return Page();
        }
        public IActionResult OnPost()
        {

            CarregarLists(int.Parse(Request.Form["IdPerfil"]));
            return Page();
        }

        #endregion

        public void CarregarLists(int cargo)
        {
            if (cargo > 0)
            {
                if (cargo == 1)
                    ListCargo = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Gerente));
                if (cargo == 2)
                    ListCargo = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Supervisor));
                //if (cargo == 3)
                //    ListCargo = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Corretor));
            }

        }

    }
}
