using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class ListarCampanhaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }
        [BindProperty]
        public Campanha Entity { get; set; }

        #endregion

        #region Construtores
        public ListarCampanhaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(long Id )
        {
            CarregarLists();
            return Page();
        }

        public void CarregarLists()
        {
            ListCampanha = Business.CampanhaModel.GetList();
        }

       
        #endregion
    }
}
