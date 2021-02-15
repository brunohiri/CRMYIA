using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class TarefaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        public List<FaseProposta> ListFaseProposta { get; set; }

        [BindProperty]
        public List<Proposta> ListEntityProposta { get; set; }
        [BindProperty]
        public Proposta Entity { get; set; }
        [BindProperty]
        public List<ListaCorretorViewModel> ListCorretor { get; set; }
        #endregion

        #region Construtores
        public TarefaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            DateTime DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month);
            DateTime DataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);
            ListEntityProposta = PropostaModel.GetListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong(), DataInicial, DataFinal);

            CarregarLists();
            return Page();
        }

        public IActionResult OnGetEdit(string statusId = null, string taskId = null)
        {
            if ((!statusId.IsNullOrEmpty()) && (!taskId.IsNullOrEmpty()))
            {
                Proposta EntityProposta = PropostaModel.Get(taskId.ExtractLong());
                EntityProposta.IdFaseProposta = statusId.ExtractByteOrNull();

                PropostaModel.Update(EntityProposta);
            }

            return new JsonResult(new { status = true });
        }

        public IActionResult OnGetBuscarFasesProposta()
        {
            //public List<FaseProposta> ListFaseProposta { get; set; }
            List<FaseProposta> FaseProposta = FasePropostaModel.GetListIdDescricao();
            DateTime DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month);
            DateTime DataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);
            ListEntityProposta = PropostaModel.GetListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong(), DataInicial, DataFinal);

            return new JsonResult(new { status = true, FaseProposta = FaseProposta, Proposta = ListEntityProposta });
        }

        public IActionResult OnGetObterHashId(string Id)
        {
            var HashId = HttpUtility.UrlDecode(Criptography.Encrypt(Id.ToString()));
            return new JsonResult(new { hashId = HashId });
        }

        public IActionResult OnGetTodasOperadoras()
        {
            List<Operadora> EntityOperadora = OperadoraModel.GetList();

            return new JsonResult(new { status = true, operadora = EntityOperadora });
        }

        //public IActionResult OnGetTodasCorretores()
        //{
        //    List<Corretora> EntityCorretora = CorretoraModel.GetList();

        //    return new JsonResult(new { status = true, corretora = EntityCorretora });
        //}


        public IActionResult OnGetPesquisaTarefa(string? Nome, string? Descricao, string? Inicio, string? Fim)
        {
            long IdUsuario = GetIdUsuario();

            DateTime? DataInicial;
            DateTime? DataFinal;
            bool Data;
            if (Inicio == null && Fim == null)
                Data = false;
            else
                Data = true;
            if (Data)
            {
                DataInicial = Convert.ToDateTime(Inicio);
                DataFinal = Convert.ToDateTime(Fim);
            }
            else
            {
                DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month);
                DataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);
            }
            Nome = Nome == "" ? null : Nome;
            Descricao = Descricao == "" ? null : Descricao;
            List<Proposta> Proposta = PropostaModel.Pesquisa(Nome, Descricao, DataFinal, DataInicial, IdUsuario);
            List<FaseProposta> FaseProposta = FasePropostaModel.GetListIdDescricao();
            return new JsonResult(new { status = true, faseProposta = FaseProposta, proposta = Proposta });
        }

        #region Abordagem
        public IActionResult OnGetAbordagem(string IdAbordagemCategoria = null, string Ordem = null, string Direcao = null)
        {
            Abordagem EntityAbordagem = null;

            if ((IdAbordagemCategoria != "undefined") && (IdAbordagemCategoria != "undenfined"))
            {
                if (Direcao == "NEXT")
                    EntityAbordagem = AbordagemModel.GetNext(IdAbordagemCategoria.ExtractByteOrZero(), Ordem.ExtractByteOrZero());
                else if (Direcao == "PREV")
                    EntityAbordagem = AbordagemModel.GetPrevious(IdAbordagemCategoria.ExtractByteOrZero(), Ordem.ExtractByteOrZero());
                else
                    EntityAbordagem = AbordagemModel.Get(1);
            }

            return new JsonResult(new { status = true, entityAbordagem = EntityAbordagem });
        }
        #endregion
        #endregion

        public void CarregarLists()
        {
           ListCorretor = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Corretor));
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
