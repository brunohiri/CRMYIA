using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class LoginModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public string LoginEmail { get; set; }
        [BindProperty]
        public string LoginSenha { get; set; }
        #endregion

        #region Construtores
        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Campos inválidos!");
            else
            {
                var IpCliente = HttpContext.Connection.RemoteIpAddress.ToString();

                Usuario EntityUsuario = UsuarioModel.Autenticar(LoginEmail, LoginSenha, IpCliente);

                if (EntityUsuario == null)
                    Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Login e/ou Senha incorretos!");
                else
                if (!EntityUsuario.Ativo)
                    Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Usuário inativo!");
                else
                if (EntityUsuario.IdCorretoraNavigation != null && !EntityUsuario.IdCorretoraNavigation.Ativo)
                    Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Cliente inativo!");
                else
                {
                    #region Autenticação
                    var userClaims = new List<Claim>()
                    {
                        //define o cookie
                        new Claim(ClaimTypes.Name, EntityUsuario.Nome),
                        new Claim(ClaimTypes.Email, EntityUsuario.Email),
                        new Claim(ClaimTypes.PrimarySid, EntityUsuario.IdUsuario.ToString())
                    };
                    var minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
                    var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });

                    //Cria o Cookie
                    HttpContext.SignInAsync(userPrincipal);
                    #endregion

                    #region Módulos
                    List<Modulo> ListEntity = ModuloModel.GetList(EntityUsuario.IdUsuario);

                    if ((ListEntity == null) || (ListEntity.Count == 0))
                    {
                        Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Erro, "Não foi possível carregar o menu de acesso!");
                    }
                    else
                    {
                        HttpContext.Session.SetObjectAsJson("MODULO", ListEntity);
                        string UrlPrincipal = ListEntity.Where(x => x.Url.ToUpper().Contains("INDEX")).FirstOrDefault().Url;
                        HistoricoAcessoModel.Add(new HistoricoAcesso()
                        {
                            DataAcesso = DateTime.Now,
                            IdUsuario = EntityUsuario.IdUsuario,
                            IP = IpCliente
                        });

                        return RedirectToPage(UrlPrincipal);
                    }
                    #endregion
                }

            }
            return Page();
        }

        #endregion
    }
}
