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

        public IActionResult OnGetStatusUsuario(int Status)
        {
            string retorno = "";
            var statusChat = EnumeradorModel.StatusChat.Invisivel;
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            switch (Status)
            {
                case 0:
                    statusChat = EnumeradorModel.StatusChat.Ativo;
                    break;
                case 1:
                    statusChat = EnumeradorModel.StatusChat.Ausente;
                    break;
                case 2:
                    statusChat = EnumeradorModel.StatusChat.NaoIncomodar;
                    break;
                case 3:
                    statusChat = EnumeradorModel.StatusChat.Invisivel;
                    break;
                default:
                    break;
            }
            retorno = Business.Util.Util.SetStatusChat(statusChat);
            if(retorno != "")
            {
                 UsuarioModel.AtualizarStatusUsuario(IdUsuario, retorno);

                return new JsonResult(new { status = true, retorno = retorno});
            }
            else
            {
                return new JsonResult(new { status = false});
            }
        }

        public IActionResult OnGetObterStatusUsuario()
        {
            string retorno = "";
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            retorno = UsuarioModel.ObterStatusUsuario(IdUsuario);
            if(retorno != "")
            {
                return new JsonResult(new { status = true, retorno = retorno });
            }
            else
            {
                return new JsonResult(new { status = false});
            }
            
        }

    }
}
