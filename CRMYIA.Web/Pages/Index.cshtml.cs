using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Dashboard;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            long IdUsuario = GetIdUsuario();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetQuantificadores((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetProducao(string? Inicio, string? Fim)
        {
            long IdUsuario = GetIdUsuario();

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

            DashboardViewModel EntityDashboard = DashboardViewModel.GetProducao((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario, DataInicial, DataFinal);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetRankings()
        {
            long IdUsuario = GetIdUsuario();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetRankings((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetRankingUsuarioCorretoresAniversariantes()
        {
            long IdUsuario = GetIdUsuario();

            DashboardViewModel EntityDashboard = DashboardViewModel.GetRankingUsuarioCorretoresAniversariantes((EnumeradorModel.Perfil)(UsuarioPerfilModel.Get(IdUsuario).IdPerfil), IdUsuario);

            return new JsonResult(new { status = true, entityDashboard = EntityDashboard });
        }

        public IActionResult OnGetPropostasPendentes()
        {
            long IdUsuario = GetIdUsuario();

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
            return new JsonResult(new { status = true, entityUsuario = Entity });
        }

        public IActionResult OnGetStatusUsuario(int Status)
        {
            string retorno = "";
            var statusChat = EnumeradorModel.StatusChat.Invisivel;
            long IdUsuario = GetIdUsuario();

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
            if (retorno != "")
            {
                UsuarioModel.AtualizarStatusUsuario(IdUsuario, retorno);

                return new JsonResult(new { status = true, retorno = retorno });
            }
            else
            {
                return new JsonResult(new { status = false });
            }
        }

        public IActionResult OnGetObterStatusUsuario()
        {
            string retorno = "";
            long IdUsuario = GetIdUsuario();
            retorno = UsuarioModel.ObterStatusUsuario(IdUsuario);
            if (retorno != "")
            {
                return new JsonResult(new { status = true, retorno = retorno });
            }
            else
            {
                return new JsonResult(new { status = false });
            }

        }

        public IActionResult OnPostLoginSlave([FromBody] Usuario obj)
        {
            string message = "";
            var claimsIdentity = (ClaimsIdentity)(ClaimsIdentity)User.Identity;
            //Add
            claimsIdentity.AddClaim(new Claim("IdUsuarioSlave", obj.IdUsuario.ToString()));
            var userPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
            //Cria o Cookie
            HttpContext.SignInAsync(userPrincipal);


            #region Módulos
            var IpCliente = HttpContext.Connection.RemoteIpAddress.ToString();
            long IdUsuario = HttpContext.User.FindFirst("IdUsuarioSlave").Value.ExtractLong();
            string UrlPrincipal = "";
            List<Modulo> ListEntity = ModuloModel.GetList(IdUsuario);
            if ((ListEntity == null) || (ListEntity.Count == 0))
            {
                message = "Erro, ao Efetuar o Login, contate o Administrador do Sistema.";
            }
            else
            {
                message = "Login Efetuado com Sucesso.";
                UrlPrincipal = ListEntity.Where(x => x.Url.ToUpper().Contains("INDEX")).FirstOrDefault().Url;
                HistoricoAcessoModel.Add(new HistoricoAcesso()
                {
                    DataAcesso = DateTime.Now,
                    IdUsuario = IdUsuario,
                    IP = IpCliente
                });
            }
            #endregion

            if (HttpContext.User.FindFirst("IdUsuarioSlave").Value != null)
            {
                return new JsonResult(new { status = true, id = HttpContext.User.FindFirst("IdUsuarioSlave").Value, url = UrlPrincipal, message = message });
            }
            else
            {
                return new JsonResult(new { status = false, message = message });
            }

        }

        public IActionResult OnPostLogoutSlave()
        {
            string message = "";
            var user = User as ClaimsPrincipal;
            var identity = user.Identity as ClaimsIdentity;
            var claim = (from c in user.Claims
                         where c.Type == "IdUsuarioSlave"
                         select c).FirstOrDefault();
            identity.RemoveClaim(claim);

            string Name = HttpContext.User.FindFirst(ClaimTypes.Name).Value.ToString();
            string Email = HttpContext.User.FindFirst(ClaimTypes.Email).Value.ToString();
            string PrimarySid = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ToString();
            
            HttpContext.SignOutAsync();
           
            //#region Autenticação
            var userClaims = new List<Claim>()
                    {
                        //define o cookie
                        new Claim(ClaimTypes.Name, Name),
                        new Claim(ClaimTypes.Email, Email),
                        new Claim(ClaimTypes.PrimarySid, PrimarySid)
                    };
            var minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
            var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });
            //Cria o Cookie
            HttpContext.SignInAsync(userPrincipal);
            #region Módulos
            var IpCliente = HttpContext.Connection.RemoteIpAddress.ToString();
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            string UrlPrincipal = "";

            List<Modulo> ListEntity = ModuloModel.GetList(IdUsuario);
            if ((ListEntity == null) || (ListEntity.Count == 0))
            {
                //Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Erro, "Não foi possível carregar o menu de acesso!");
                message = "Erro, ao Efetuar o Login, contate o Administrador do Sistema.";
            }
            else
            {
                message = "Login Efetuado com Sucesso.";
                UrlPrincipal = ListEntity.Where(x => x.Url.ToUpper().Contains("INDEX")).FirstOrDefault().Url;
                HistoricoAcessoModel.Add(new HistoricoAcesso()
                {
                    DataAcesso = DateTime.Now,
                    IdUsuario = IdUsuario,
                    IP = IpCliente
                });
            }
            #endregion
            if (HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value != null)
            {
                return new JsonResult(new { status = true, id = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value, url = UrlPrincipal, message = message });
            }
            else
            {
                return new JsonResult(new { status = false, message = message });
            }
        }

        public long GetIdUsuario()
        {
            long IdUsuario = "0".ExtractLong();

            if (HttpContext.User.Equals("IdUsuarioSlave"))
            {
                IdUsuario = HttpContext.User.FindFirst("IdUsuarioSlave").Value.ExtractLong();
            }
            else
            {
                IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            }
            return IdUsuario;
        }


    }
}
