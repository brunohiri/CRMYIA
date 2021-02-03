using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ListarPropostaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<ListaPropostaViewModel> ListEntity { get; set; }
        public List<FaseProposta> ListFaseProposta { get; set; }
        #endregion

        #region Construtores
        public ListarPropostaModel(IConfiguration configuration)
        {                   
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            ListEntity = PropostaModel.GetList(GetIdUsuario());
            ListFaseProposta = FasePropostaModel.GetList();
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        #endregion

        public long GetIdUsuario()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> Claims = identity.Claims;
            foreach (var t in Claims)
            {
                if (t.Type.Equals("IdUsuarioSlave"))
                    IdUsuario = t.Value.ExtractLong();
            }
            return IdUsuario;
        }
    }
}
