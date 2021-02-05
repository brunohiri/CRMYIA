using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ListarAniversarianteModel : PageModel
    {

        #region Propriedades
        readonly IConfiguration _configuration;
        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<ListaAniversarianteViewModel> ListEntity { get; set; }
        #endregion

        #region Construtores
        public ListarAniversarianteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion
        #region Métodos
        public IActionResult OnGet()
        {
            ListEntity = UsuarioModel.GetListAniversariante();//GetListAniversariante
            return Page();
        }
        #endregion
    }
}
