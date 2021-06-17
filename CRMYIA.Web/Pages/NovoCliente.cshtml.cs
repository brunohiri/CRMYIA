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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class NovoClienteModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Cliente Entity { get; set; }

        #region TipoLead
        [BindProperty]
        public byte? IdTipoLead { get; set; }

        [BindProperty]
        public List<TipoLead> ListTipoLead { get; set; }
        #endregion

        #region Origem
        [BindProperty]
        public byte? IdOrigem { get; set; }

        [BindProperty]
        public List<Origem> ListOrigem { get; set; }
        #endregion

        #region Genero
        [BindProperty]
        public byte? IdGenero { get; set; }

        [BindProperty]
        public List<Genero> ListGenero { get; set; }
        #endregion

        #region EstadoCivil
        [BindProperty]
        public byte? IdEstadoCivil { get; set; }

        [BindProperty]
        public List<EstadoCivil> ListEstadoCivil { get; set; }
        #endregion

        #region Cidade/Estado
        [BindProperty]
        public List<Cidade> ListCidade { get; set; }

        [BindProperty]
        public List<Estado> ListEstado { get; set; }
        #endregion

        #region Email
        [BindProperty]
        public Email EntityEmail { get; set; }
        #endregion

        #region Telefone
        [BindProperty]
        public Telefone EntityTelefone { get; set; }

        [BindProperty]
        public List<OperadoraTelefone> ListOperadoraTelefone { get; set; }
        #endregion

        #region Operadora
        public long? IdOperadora { get; set; }
        [BindProperty]
        public List<Operadora> ListOperadora { get; set; }

        [BindProperty]
        public List<Modalidade> ListModalidade { get; set; }
        #endregion
        #endregion

        #region Construtores
        public NovoClienteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
            {
                Entity = new Cliente();
            }
            else
            {
                Entity = ClienteModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            }

            Entity.DataCadastro = DateTime.Now;
            CarregarLists();

            #region Telefone
            EntityTelefone = new Telefone();
            EntityTelefone.DataCadastro = DateTime.Now;
            #endregion

            #region Email
            EntityEmail = new Email();
            EntityEmail.DataCadastro = DateTime.Now;
            #endregion

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                CarregarLists();

                if ((!Util.IsCpf(Entity.CPF)) && (!Util.IsCnpj(Entity.CPF)))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CPF ou CNPJ Inválido!");
                else
                {
                    if (Entity.IdCliente == 0)
                    {
                        if (ClienteModel.GetByCPF(Entity.CPF) != null)
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um cliente cadastrado com este CPF/CNPJ!");
                        else
                        {
                            ClienteModel.Add(Entity);
                            UsuarioClienteModel.Add(new UsuarioCliente { 
                                IdUsuario = GetIdUsuario(), 
                                IdCliente = Entity.IdCliente, 
                                DataCadastro = DateTime.Now, 
                                Ativo = true
                            });
                            UsuarioClienteModel.DesativarUltimoCorretor(Entity.IdCliente, GetIdUsuario());
                        }
                    }
                    else
                    {
                        ClienteModel.Update(Entity);
                        UsuarioClienteModel.Add(new UsuarioCliente
                        {
                            IdUsuario = GetIdUsuario(),
                            IdCliente = Entity.IdCliente,
                            DataCadastro = DateTime.Now,
                            Ativo = true
                        });
                        UsuarioClienteModel.DesativarUltimoCorretor(Entity.IdCliente, GetIdUsuario());
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

        #region Telefone
        public IActionResult OnGetListTelefone(string Id = null)
        {
            List<Telefone> ListEntityTelefone = null;
            if (Id.IsNullOrEmpty())
                ListEntityTelefone = new List<Telefone>();
            else
            {
                ListEntityTelefone = TelefoneModel.GetList(Id.ExtractLong());
            }

            return new JsonResult(new { listEntityTelefone = ListEntityTelefone });
        }

        public IActionResult OnGetTelefone(string Id)
        {
            Telefone EntityEditarTelefone = null;
            EntityEditarTelefone = TelefoneModel.Get(Id.ExtractLong());

            return new JsonResult(new { entityEditarTelefone = EntityEditarTelefone });
        }

        public IActionResult OnPostTelefone()
        {
            try
            {
                EntityTelefone.DataCadastro = DateTime.Now;
                EntityTelefone.DDD = EntityTelefone.Telefone1.KeepOnlyNumbers().Substring(0, 2);
                EntityTelefone.Telefone1 = EntityTelefone.Telefone1.KeepOnlyNumbers().Substring(2, EntityTelefone.Telefone1.KeepOnlyNumbers().Length - 2);
                
                if (string.IsNullOrEmpty(EntityTelefone.Telefone1))
                    throw new Exception("Formato de Telefone Inválido!");

                if (EntityTelefone.IdOperadoraTelefone == null)
                    throw new Exception("Selecione uma das Operadora da Lista.");

                if (EntityTelefone.IdTelefone == 0)
                {
                    TelefoneModel.Add(EntityTelefone);
                }
                else
                {
                    TelefoneModel.Update(EntityTelefone);
                }
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");

            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return new JsonResult(new { mensagem = Mensagem });
        }
        #endregion

        #region Email
        public IActionResult OnGetListEmail(string Id = null)
        {
            List<Email> ListEntityEmail = null;
            if (Id.IsNullOrEmpty())
                ListEntityEmail = new List<Email>();
            else
            {
                ListEntityEmail = EmailModel.GetList(Id.ExtractLong());
            }

            return new JsonResult(new { listEntityEmail = ListEntityEmail });
        }

        public IActionResult OnGetEmail(string Id)
        {
            Email EntityEditarEmail = null;
            EntityEditarEmail = EmailModel.Get(Id.ExtractLong());

            return new JsonResult(new { entityEditarEmail = EntityEditarEmail });
        }

        public IActionResult OnPostEmail()
        {
            try
            {
                if (string.IsNullOrEmpty(EntityEmail.EmailConta))
                    throw new Exception("O Email não pode ser vazio.");

                if (EntityEmail.IdEmail == 0)
                {
                    EmailModel.Add(EntityEmail);
                }
                else
                {
                    EntityEmail.DataCadastro = DateTime.Now;
                    EmailModel.Update(EntityEmail);
                }
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");

            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return new JsonResult(new { mensagem = Mensagem });
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
        #endregion
        #endregion

        #region Outros Métodos
        private void CarregarLists()
        {
            ListTipoLead = TipoLeadModel.GetListIdDescricao();
            ListOrigem = OrigemLeadModel.GetListIdDescricao();
            ListGenero = GeneroModel.GetListIdDescricao();
            ListEstadoCivil = EstadoCivilModel.GetListIdDescricao();
            ListCidade = CidadeModel.GetListIdDescricao();
            ListEstado = EstadoModel.GetListIdSigla();
            ListOperadoraTelefone = OperadoraTelefoneModel.GetListIdDescricao();
            ListOperadora = OperadoraModel.GetListIdDescricao();
            ListModalidade = ModalidadeModel.GetList();
        }
        #endregion
    }
}
