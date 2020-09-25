using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class NovoCorretorModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Usuario Entity { get; set; }

        [BindProperty]
        public string ConfirmarSenha { get; set; }

        [BindProperty]
        public List<Corretora> ListCorretora { get; set; }

        #endregion

        #region Construtores
        public NovoCorretorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Usuario();
            else
            {
                Entity = UsuarioModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ConfirmarSenha = Entity.Senha;
            }

            Entity.DataCadastro = DateTime.Now;
            ListCorretora = CorretoraModel.GetListIdDescricao();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                ListCorretora = CorretoraModel.GetListIdDescricao();

                if (!Util.IsCpf(Entity.CPF))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CPF Inv�lido!");
                else
                {
                    if (!Entity.Senha.IsNullOrEmpty())
                    {
                        EnumeradorModel.PasswordStrength classicacaoSenha = Util.GetPasswordStrength(Entity.Senha);
                        if ((classicacaoSenha == EnumeradorModel.PasswordStrength.Aceitavel)
                            || (classicacaoSenha == EnumeradorModel.PasswordStrength.Forte)
                            || (classicacaoSenha == EnumeradorModel.PasswordStrength.Segura))
                        {
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, string.Format("Senha {0}! Utilize n�meros, caracteres especiais e letras mai�sculas e min�sculas!", classicacaoSenha.ToString()));
                        }
                        else
                        if (Entity.Senha != ConfirmarSenha)
                        {
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Senha n�o � compat�vel com a confirma��o de senha!");
                        }
                    }

                    if (Mensagem == null)
                    {
                        Entity.IP = HttpContext.Connection.RemoteIpAddress.ToString();

                        if (Entity.IdUsuario == 0)
                        {
                            if (UsuarioModel.GetByCPF(Entity.CPF) != null)
                                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "J� existe um usu�rio cadastrado com este CPF!");
                            else
                            if (UsuarioModel.GetByLogin(Entity.Login) != null)
                                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "J� existe um usu�rio cadastrado com este Login!");
                            else
                            {
                                UsuarioModel.Add(Entity);
                                UsuarioPerfilModel.Add(new UsuarioPerfil() { IdUsuario = Entity.IdUsuario, IdPerfil = (byte)EnumeradorModel.Perfil.Corretor, Ativo = true });
                            }
                        }
                        else
                        {
                            if (!Entity.Senha.IsNullOrEmpty())
                                Entity.Senha = Criptography.Encrypt(Entity.Senha);
                            UsuarioModel.Update(Entity);
                        }

                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion
    }
}
