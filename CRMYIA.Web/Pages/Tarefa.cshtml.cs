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
using Microsoft.AspNetCore.Http;
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
        public List<List<Proposta>> ListListEntityProposta { get; set; }
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
            ListListEntityProposta = PropostaModel.GetListListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong(), DataInicial, DataFinal, "", "", 0, 0);

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

        public IActionResult OnPostPesquisaTarefa(IFormCollection dados)
        {
            string Nome, Descricao, Inicio, Fim;
            DateTime DataInicial, DataFinal;
            bool status = false;
            long IdUsuario = GetIdUsuario();


            Nome = dados["Nome"];
            Descricao = dados["Descricao"];
            Inicio = dados["Inicio"];
            Fim = dados["Fim"];
            int.TryParse(dados["Salto"], out int Salto);
            byte.TryParse(dados["Fase"], out byte Fase);
            List<FaseProposta> FaseProposta = FasePropostaModel.GetListIdDescricao();

            DataInicial = Inicio == "" ? Util.GetFirstDayOfMonth(DateTime.Now.Month) : Convert.ToDateTime(Inicio);
            DataFinal = Fim == "" ? Util.GetLastDayOfMonth(DateTime.Now.Month) : Convert.ToDateTime(Fim);

            ListListEntityProposta = PropostaModel.GetListListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong(), DataInicial, DataFinal, Nome, Descricao, Fase, Salto);
            if (ListListEntityProposta[0].Count > 0)
                status = true;
            return new JsonResult(new { status, Fase, FaseProposta, Propostas = ListListEntityProposta, periodoA = DataInicial.ToString(), periodoB = DataFinal.ToString() });


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
