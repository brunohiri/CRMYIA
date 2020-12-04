﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
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
        public List<Usuario> ListCorretor { get; set; }
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
            DateTime DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month - 1);
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
            DateTime DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month - 1);
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


        public IActionResult OnGetPesquisaTarefa(long? IdOperadora, long? IdUsuarioCorretor, string Inicio, string Fim)
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            DateTime DataInicial = Convert.ToDateTime(Inicio);
            DateTime DataFinal = Convert.ToDateTime(Fim);
            IdOperadora = IdOperadora == 0 ?null : IdOperadora;
            IdUsuarioCorretor = IdUsuarioCorretor == 0 ? null : IdUsuarioCorretor;
            List<Proposta> Proposta = PropostaModel.Pesquisa(IdOperadora, IdUsuarioCorretor, DataFinal, DataInicial, IdUsuario);
            List<FaseProposta> FaseProposta = FasePropostaModel.GetListIdDescricao();
            return new JsonResult(new { status = true, faseProposta = FaseProposta, proposta = Proposta });
        }
        #endregion

        public void CarregarLists()
        {
            ListCorretor = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Corretor));
        }

    }
}
