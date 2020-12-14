using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class ListarNotificacaoModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;
        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<Notificacao> ListEntity { get; set; }
        #endregion

        #region Construtores
        public ListarNotificacaoModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion
        #region Métodos
        public IActionResult OnGet()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            ListEntity = NotificacaoModel.GetListNotificacao(IdUsuario);
            return Page();
        }
        #endregion
    }
}
