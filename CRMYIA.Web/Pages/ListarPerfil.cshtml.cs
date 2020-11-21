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
    public class ListarPerfilModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<Perfil> ListEntity { get; set; }
        #endregion

        #region Construtores
        public ListarPerfilModel(IConfiguration configuration)
        {                   
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            ListEntity = PerfilModel.GetList();
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        #endregion
    }
}