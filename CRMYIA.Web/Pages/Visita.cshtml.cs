using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class VisitaModel : PageModel
    {
        #region Propriedades

        #region Lists
        public List<StatusVisita> ListStatusVisita { get; set; }
        #endregion
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            ListStatusVisita = StatusVisitaModel.GetList();
            return Page();
        }

        public IActionResult OnGetVisitas()
        {
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Util.GetFirstDayOfMonth(DateTime.Now.Month), Util.GetLastDayOfMonth(DateTime.Now.Month));

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }

        #endregion
    }
}
