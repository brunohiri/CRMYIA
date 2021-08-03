using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
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

        [BindProperty]
        public List<Classificacao> ListClassificacao { get; set; }

        [BindProperty]
        public List<Producao> ListProducao { get; set; }
        [BindProperty]
        public List<UsuarioViewModel> ListSupervisor { get; set; }
        [BindProperty]
        public List<Banco> ListBanco { get; set; }
        #endregion

        #region Construtores
        public NovoCorretorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
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
            ListClassificacao = ClassificacaoModel.GetListIdDescricao();
            ListProducao = ProducaoModel.GetListIdDescricao();
            ListSupervisor = UsuarioPerfilModel.GetList((byte)(EnumeradorModel.Perfil.Supervisor));
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                ListCorretora = CorretoraModel.GetListIdDescricao();
                ListClassificacao = ClassificacaoModel.GetListIdDescricao();
                ListProducao = ProducaoModel.GetListIdDescricao();

                if ((!Util.IsCpf(Entity.Documento)) && (!Util.IsCnpj(Entity.Documento)))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CPF ou CNPJ Inválido!");
                else
                {
                    Entity.IP = HttpContext.Connection.RemoteIpAddress.ToString();

                    if (Entity.IdUsuario == 0)
                    {
                        if (UsuarioModel.GetByDocumento(Entity.Documento) != null)
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um usuário cadastrado com este CPF ou CNPJ!");
                        else
                        if (UsuarioModel.GetByLogin(Entity.Login) != null)
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um usuário cadastrado com este Login!");
                        else
                        {
                            UsuarioModel.Add(Entity);
                            UsuarioPerfilModel.Add(new UsuarioPerfil() { IdUsuario = Entity.IdUsuario, IdPerfil = (byte)EnumeradorModel.Perfil.Corretor, Ativo = true });
                            UsuarioHierarquiaModel.Add(new UsuarioHierarquia() { Ativo = true, DataCadastro = DateTime.Now, IdUsuarioMaster = Entity.Superior, IdUsuarioSlave = Entity.IdUsuario });

                            //fazer html do email
                           /// MailModel.SendMail();
                        }
                    }
                    else
                    {
                        UsuarioModel.Update(Entity);
                    }

                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
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
