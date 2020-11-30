using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class TarefaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        public List<FaseProposta> ListFaseProposta { get; set; }

        [BindProperty]
        public List<Proposta> ListEntityProposta { get; set; }
        #endregion

        #region Construtores
        public TarefaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            ListEntityProposta = PropostaModel.GetListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong());

            return Page();
        }

        public IActionResult OnGetEdit(string statusId = null, string taskId = null)
        {
            if ((!statusId.IsNullOrEmpty()) && (!taskId.IsNullOrEmpty()))
            {
                Proposta EntityProposta = PropostaModel.Get(taskId.ExtractLong());
                EntityProposta.IdFaseProposta = statusId.ExtractByteOrNull();

                PropostaModel.Update(EntityProposta);
            }
            return new JsonResult(new { status = true });
        }
        public IActionResult OnGetListarFaseProposta()
        {
            //public List<FaseProposta> ListFaseProposta { get; set; }
            List<FaseProposta> FaseProposta = FasePropostaModel.GetListIdDescricao();
            List<Proposta> Proposta = PropostaModel.GetListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong());

            return new JsonResult(new { status = true, FaseProposta = FaseProposta, Proposta = Proposta });
        }
        #endregion
    }
}
