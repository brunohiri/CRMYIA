using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Dashboard;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<List<Modulo>>("MODULO") == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToPage("Login");
            }
            return Page();
        }


        public IActionResult OnGetQuantificadores()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetQuantificadores((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status=true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetProducao(string? Inicio, string? Fim)
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DateTime? DataInicial;
            DateTime? DataFinal;
            
            if (Inicio == null && Fim == null)
            {
                DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month);
                DataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);
            }
            else
            {
                DataInicial = Convert.ToDateTime(Inicio);
                DataFinal = Convert.ToDateTime(Fim);
            }

            DashboardViewModel EntityDashboard = DashboardViewModel.GetProducao((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario, DataInicial , DataFinal);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetRankings()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetRankings((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetRankingUsuarioCorretoresAniversariantes()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetRankingUsuarioCorretoresAniversariantes((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetPropostasPendentes()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetPropostasPendentes((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }
        public IActionResult OnGetObterHashId(string Id)
        {
            var HashId = HttpUtility.UrlEncode(Criptography.Encrypt(Id));
            return new JsonResult(new { hashId = HashId });
        }

        public IActionResult OnGetPesquisaChat(string parametro)
        {
            List<Usuario> Entity = null;
            Entity = UsuarioModel.GetAll(parametro);
            return new JsonResult(new { status = true , entityUsuario = Entity });
        }

        public IActionResult OnGetCarregaMensagem(long Para, long De, int Limit = 0, int NumeroDeMensagem = 0)
        {
            List<Chat> ListChat = null;
            ListChat = ChatModel.CarregaMensagem(Para, De, Limit, NumeroDeMensagem);
            return new JsonResult(new { status = true, listChat = ListChat});
        }
    }
}
