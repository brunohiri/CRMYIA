using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class MinhaContaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        #region Usuario
        [BindProperty]
        public Usuario Entity { get; set; }
        [BindProperty]
        public Perfil EntityCargo { get; set; }
        public long IdUsuario { get; set; }
        public UsuarioPerfil UsuarioPerfil { get; set; }
        public string ConfirmarSenha { get; set; }
        #endregion
        #endregion

        #region Construtores
        public MinhaContaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            UsuarioPerfil = PerfilModel.GetIdentificacaoPerfil(IdUsuario);

            Entity = UsuarioModel.Get(IdUsuario);
            EntityCargo = PerfilModel.Get(UsuarioPerfil.IdPerfil.Value);
            //if (Id.IsNullOrEmpty())
            //{
            //    Entity = new KPIMeta();
            //}
            //else
            //{
            //    idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();

            //    Mes = DateTime.Now.Month;

            //    EntityKPIGrupo = KPIGrupoModel.Get(idUser);
            //    Entity = KPIMetaModel.Get((long)EntityKPIGrupo.IdKPIGrupo);
            //    if (Entity != null)
            //    {
            //        KPIMetaValorEntity = KPIMetaValorModel.Get(Entity.IdMeta);
            //        KPIMetaVidaEntity = KPIMetaVidaModel.Get(Entity.IdMeta);
            //    }
            //    else
            //    {
            //        Entity = new KPIMeta();
            //    }
            //}
            //CarregarLists();
            return Page();
        }
        public IActionResult OnPostRedesSociais(IFormCollection dados)
        {
            long id = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            Usuario usuario = new Usuario();
            usuario = UsuarioModel.Get(id);
            if (!usuario.Nome.IsNullOrEmpty() && !usuario.Nome.IsNullOrEmpty())
            {
                usuario.Facebook = dados["Facebook"];
                usuario.Instagram = dados["Instagram"];
                usuario.Linkedin = dados["Linkedin"];
                usuario.Twitter = dados["Twitter"];
                UsuarioModel.Update(usuario);
                status = true;
            }
            if (status == false)
                return new JsonResult(new { mensagem = "Ocorreu um erro, contate o administrador do sistema!", status });
            else
                return new JsonResult(new { mensagem = "Sucesso", status });
        }
        public IActionResult OnPostAlterarSenha(IFormCollection dados)
        {
            long id = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            ConfirmarSenha = dados["SenhaAtual"];
            if (!ConfirmarSenha.IsNullOrEmpty())
                ConfirmarSenha = Criptography.Encrypt(ConfirmarSenha);
            Usuario usuario = new Usuario();
            usuario = UsuarioModel.Get(id);

            if (id > 0 && usuario != null)
            {
                EnumeradorModel.PasswordStrength classicacaoSenha = Util.GetPasswordStrength(dados["Senha"]);
                if ((classicacaoSenha == EnumeradorModel.PasswordStrength.Aceitavel)
                    || (classicacaoSenha == EnumeradorModel.PasswordStrength.Forte)
                    || (classicacaoSenha == EnumeradorModel.PasswordStrength.Segura))
                {
                    return new JsonResult(new
                    {
                        mensagem = new MensagemModel(
                        Business.Util.EnumeradorModel.TipoMensagem.Aviso,
                        string.Format("Senha {0}! Utilize números, caracteres especiais e letras maiúsculas e minúsculas!", classicacaoSenha.ToString())
                        )
                    });
                }
                else if (usuario.Senha != ConfirmarSenha)
                {
                    return new JsonResult(new { mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Senha não é compatível com a confirmação de senha!") });
                }
                else
                {
                    usuario.Senha = ConfirmarSenha;
                    UsuarioModel.Update(usuario);
                    status = true;
                }
            }
            if (status == false)
                return new JsonResult(new { mensagem = "Erro ao alterar sua senha, contate o administrador do sistema!", status });
            else
                return new JsonResult(new { mensagem = "Sucesso", status });
        }
        public IActionResult OnPostAlterarDados(IFormCollection dados)
        {
            long id = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            Usuario usuario = new Usuario();
            usuario = UsuarioModel.Get(id);
            if (!usuario.Nome.IsNullOrEmpty() && !usuario.Nome.IsNullOrEmpty())
            {
                usuario.Nome = dados["Nome"];
                usuario.NomeApelido = dados["NomeApelido"];
                usuario.Telefone = dados["Telefone"];
                UsuarioModel.Update(usuario);
                status = true;
            }
            if (status == false)
                return new JsonResult(new { mensagem = "Erro ao alterar seus dados, contate o administrador do sistema!", status });
            else
                return new JsonResult(new { mensagem = "Sucesso", status });
        }
        public IActionResult OnPost()
        {
            return Page();
        }

        #region Outros Métodos
        private void CarregarLists()
        {
            //ListUsuario = UsuarioModel.GetList();

        }
        #endregion

        #endregion
    }
}
