using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class ListarKPIMetasModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<KPIMetaValor> ListEntityValor { get; set; }
        [BindProperty]
        public List<KPIMetaVida> ListEntityVida { get; set; }

        #endregion

        #region Construtores
        public ListarKPIMetasModel(IConfiguration configuration)
        {                   
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            ListEntityValor = KPIMetaValorModel.GetList();
            ListEntityVida = KPIMetaVidaModel.GetList();

            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        #endregion
    }
}
