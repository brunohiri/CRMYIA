using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
    public class NovaPropostaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Proposta Entity { get; set; }

        public List<PropostaFaixaEtaria> ListEntityPropostaFaixaEtaria { get; set; }

        public List<HistoricoProposta> ListEntityHistoricoProposta { get; set; }

        [BindProperty]
        public string[] PropostaFaixaEtariaQuantidade { get; set; }
        [BindProperty]
        public string[] PropostaFaixaEtariaId { get; set; }

        #region Lists
        [BindProperty]
        public List<Usuario> ListCorretor { get; set; }

        [BindProperty]
        public List<Cliente> ListCliente { get; set; }

        [BindProperty]
        public List<FaseProposta> ListFaseProposta { get; set; }

        [BindProperty]
        public List<StatusProposta> ListStatusProposta { get; set; }

        [BindProperty]
        public List<Modalidade> ListModalidade { get; set; }

        [BindProperty]
        public List<Produto> ListProduto { get; set; }

        [BindProperty]
        public List<MotivoDeclinio> ListMotivoDeclinio { get; set; }

        [BindProperty]
        public List<FaixaEtaria> ListFaixaEtaria { get; set; }
        #endregion

        #endregion

        #region Construtores
        public NovaPropostaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
            {
                Entity = new Proposta();
                Entity.DataSolicitacao = DateTime.Now;
                Entity.ProximoContatoComCliente = DateTime.Now.AddDays(2);
            }
            else
            {
                Entity = PropostaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListEntityPropostaFaixaEtaria = PropostaFaixaEtariaModel.GetList(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListEntityHistoricoProposta = HistoricoPropostaModel.GetList(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            }

            #region Verificar se o usuário que está cadastrando a proposta é corretor
            if (UsuarioModel.GetPerfil(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong()) == (byte?)EnumeradorModel.Perfil.Corretor)
                Entity.IdUsuarioCorretor = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            #endregion

            Entity.IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            Entity.DataCadastro = DateTime.Now;

            CarregarLists();

            return Page();
        }

        public IActionResult OnGetCliente(string Id)
        {
            ClienteViewModel EntityCliente = null;
            if (Id != "undefined")
                EntityCliente = ClienteModel.GetWithCidadeEstadoTelefoneEmail(Id.ExtractLong());

            return new JsonResult(new { entityCliente = EntityCliente });
        }

        public IActionResult OnPost()
        {
            string Observacao = string.Empty;
            List<PropostaFaixaEtaria> ListPropostaFaixaEtaria = null;
            try
            {
                CarregarLists();

                if (Entity.IdMotivoDeclinio == 0)
                    Entity.IdMotivoDeclinio = null;

                if ((!PropostaFaixaEtariaQuantidade.ToList().Sum(x => x.ExtractInt32OrNull()).HasValue) && (PropostaFaixaEtariaQuantidade.ToList().Sum(x => x.ExtractInt32OrNull()).Value == 0))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Digite a quantidade de vidas pra cada faixa etária!");


                if (Entity.IdProposta == 0)
                {
                    PropostaModel.Add(Entity);

                    #region Salvar PropostaFaixaEtaria
                    ListPropostaFaixaEtaria = new List<PropostaFaixaEtaria>();

                    ListFaixaEtaria.ForEach(delegate (FaixaEtaria Item)
                        {
                            ListPropostaFaixaEtaria.Add(new PropostaFaixaEtaria()
                            {
                                IdFaixaEtaria = Item.IdFaixaEtaria,
                                IdProposta = Entity.IdProposta,
                                Quantidade = PropostaFaixaEtariaQuantidade[Item.IdFaixaEtaria - 1].ExtractInt32OrNull(),
                                Ativo = true
                            });
                        });

                    PropostaFaixaEtariaModel.AddRange(ListPropostaFaixaEtaria);
                    #endregion

                    Observacao = "Proposta Criada!";
                }
                else
                {
                    PropostaModel.Update(Entity);

                    #region Salvar PropostaFaixaEtaria
                    ListFaixaEtaria.ForEach(delegate (FaixaEtaria Item)
                    {
                        PropostaFaixaEtaria EntityPropostaFaixaEtaria = PropostaFaixaEtariaModel.Get(Entity.IdProposta, Item.IdFaixaEtaria);
                        EntityPropostaFaixaEtaria.Quantidade = PropostaFaixaEtariaQuantidade[Item.IdFaixaEtaria - 1].ExtractInt32OrNull();
                        PropostaFaixaEtariaModel.Update(EntityPropostaFaixaEtaria);
                    });
                    #endregion

                    Observacao = "Proposta Alterada!";
                }

                #region Salvar Histórico da Proposta
                HistoricoPropostaModel.Add(new HistoricoProposta()
                {
                    IdProposta = Entity.IdProposta,
                    IdUsuario = Entity.IdUsuario,
                    Observacao = Observacao + (Entity.Observacoes.IsNullOrEmpty() ? string.Empty : " " + Observacao),
                    DataCadastro = DateTime.Now,
                    Ativo = true
                });
                #endregion

                ListEntityPropostaFaixaEtaria = PropostaFaixaEtariaModel.GetList(Entity.IdProposta);
                ListEntityHistoricoProposta = HistoricoPropostaModel.GetList(Entity.IdProposta);

                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                if (ex.Source.Equals("Microsoft.EntityFrameworkCore.Relational"))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Campos obrigatórios em branco! Por favor, verifique!");
                else
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion

        #region Outros Métodos
        private void CarregarLists()
        {
            ListCorretor = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Corretor));
            ListCliente = ClienteModel.GetListIdNome();
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            ListStatusProposta = StatusPropostaModel.GetListIdDescricao();
            ListModalidade = ModalidadeModel.GetListIdDescricao();
            ListProduto = ProdutoModel.GetListIdDescricao();
            ListMotivoDeclinio = MotivoDeclinioModel.GetListIdDescricao();
            ListFaixaEtaria = FaixaEtariaModel.GetListIdDescricao();
        }
        #endregion
    }
}
