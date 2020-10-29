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

        public long? IdOperadora { get; set; }

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
        public List<Operadora> ListOperadora { get; set; }

        [BindProperty]
        public List<MotivoDeclinio> ListMotivoDeclinio { get; set; }

        [BindProperty]
        public List<FaixaEtaria> ListFaixaEtaria { get; set; }

        [BindProperty]
        public List<Porte> ListPorte { get; set; }
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
                IdOperadora = 0;
            }
            else
            {
                Entity = PropostaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListEntityPropostaFaixaEtaria = PropostaFaixaEtariaModel.GetList(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListEntityHistoricoProposta = HistoricoPropostaModel.GetList(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());

                IdOperadora = Entity.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora;
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

        public IActionResult OnGetCliente(string Id = null, string Documento = null)
        {
            ClienteViewModel EntityCliente = null;
            if ((!Id.IsNullOrEmpty()) && (Id.Replace("null", "undefined") != "undefined"))
                EntityCliente = ClienteModel.GetWithCidadeEstadoTelefoneEmailEndereco(Id.ExtractLong(), null);
            else
            if ((!Documento.IsNullOrEmpty()) && (Documento.Replace("null", "undefined") != "undefined"))
                EntityCliente = ClienteModel.GetWithCidadeEstadoTelefoneEmailEndereco(null, Documento);

            return new JsonResult(new { entityCliente = EntityCliente });
        }

        public IActionResult OnGetProduto(string IdOperadora = null, string IdProduto = null)
        {
            List<Produto> ListProduto = null;
            long? IdOperadoraProduto = 0;
            if ((IdOperadora != "undefined") && (IdProduto != "undenfined"))
            {
                if ((!IdOperadora.IsNullOrEmpty()) && (IdOperadora != "0"))
                {
                    ListProduto = ProdutoModel.GetListIdDescricaoByOperadora(IdOperadora.ExtractLong());
                }
                else
                    if ((!IdProduto.IsNullOrEmpty()) && (IdProduto != "0"))
                {
                    Produto EntityProduto = ProdutoModel.Get(IdProduto.ExtractLong());
                    IdOperadoraProduto = EntityProduto.IdOperadora;
                    ListProduto = ProdutoModel.GetListIdDescricaoByOperadora(IdOperadoraProduto.Value);
                }

            }

            return new JsonResult(new { status = true, listProduto = ListProduto, idOperadora = IdOperadoraProduto });
        }

        public IActionResult OnGetLinha(string IdProduto = null, string IdLinha = null)
        {
            List<Linha> ListLinha = null;
            long? IdProdutoLinha = 0;
            if ((IdProduto != "undefined") && (IdLinha != "undenfined"))
            {
                if ((!IdProduto.IsNullOrEmpty()) && (IdProduto != "0"))
                {
                    ListLinha = LinhaModel.GetListIdDescricaoByProduto(IdProduto.ExtractLong());
                }
                else
                    if ((!IdLinha.IsNullOrEmpty()) && (IdLinha != "0"))
                {
                    Linha EntityLinha = LinhaModel.Get(IdLinha.ExtractLong());
                    IdProdutoLinha = EntityLinha.IdProduto;
                    ListLinha = LinhaModel.GetListIdDescricaoByProduto(IdProdutoLinha.Value);
                }

            }

            return new JsonResult(new { status = true, listLinha = ListLinha, idProduto = IdProdutoLinha });
        }

        public IActionResult OnGetCategoria(string IdLinha = null, string IdCategoria = null)
        {
            List<Categoria> ListCategoria = null;
            long? IdLinhaCategoria = 0;
            if ((IdLinha != "undefined") && (IdCategoria != "undenfined"))
            {
                if ((!IdLinha.IsNullOrEmpty()) && (IdLinha != "0"))
                {
                    ListCategoria = CategoriaModel.GetListIdDescricaoByLinha(IdLinha.ExtractLong());
                }
                else
                    if ((!IdCategoria.IsNullOrEmpty()) && (IdCategoria != "0"))
                {
                    Categoria EntityCategoria = CategoriaModel.Get(IdCategoria.ExtractLong());
                    IdLinhaCategoria = EntityCategoria.IdLinha;
                    ListCategoria = CategoriaModel.GetListIdDescricaoByLinha(IdLinhaCategoria.Value);
                }

            }

            return new JsonResult(new { status = true, listCategoria = ListCategoria, idLinha = IdLinhaCategoria });
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
                    Observacao = Observacao + (Entity.Observacoes.IsNullOrEmpty() || Entity.Observacoes.Equals(Observacao) ? string.Empty : " " + Observacao),
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
            ListOperadora = OperadoraModel.GetListIdDescricao();
            ListMotivoDeclinio = MotivoDeclinioModel.GetListIdDescricao();
            ListFaixaEtaria = FaixaEtariaModel.GetListIdDescricao();
            ListPorte = PorteModel.GetListIdDescricao();
        }
        #endregion
    }
}
