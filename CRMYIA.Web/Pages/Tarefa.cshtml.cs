using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string DataInicialDefault;

        public string DataFinalDefault;

        public MensagemModel Mensagem { get; set; }

        public List<FaseProposta> ListFaseProposta { get; set; }

        [BindProperty]
        public List<List<Proposta>> ListListEntityProposta { get; set; }

        [BindProperty]
        public List<Proposta> ListEntityProposta { get; set; }

        [BindProperty]
        public Proposta Entity { get; set; }

        [BindProperty]
        public List<Usuario> ListCorretor { get; set; }

        [BindProperty]
        public UsuarioGerenteViewModel UsuarioGerente { get; set; }

        [BindProperty]
        public List<ListaCorretorViewModel> ListGerente { get; set; }

        [BindProperty]
        public UsuarioSupervisorViewModel UsuarioSupervisor { get; set; }

        public List<MotivoDeclinioLead> DeclinioLeadSelectMenu { get; set; }

        public byte? IdDeclinioLeadTemp;
        #endregion

        public class MotivoDeclinioLead
        {
            public MotivoDeclinioLead()
            {
                Proposta = new HashSet<Proposta>();
            }

            public byte IdMotivoDeclinioLead { get; set; }
            public string Descricao { get; set; }
            public bool Ativo { get; set; }

            public virtual ICollection<Proposta> Proposta { get; set; }
        }

        #region Construtores
        public TarefaModel(IConfiguration configuration)
        {
            _configuration = configuration;
            DeclinioLeadSelectMenu = new List<MotivoDeclinioLead>();
            DeclinioLeadSelectMenu.Add(new MotivoDeclinioLead { IdMotivoDeclinioLead = 1, Descricao = "Cliente não deseja mais contato", Ativo = true });
            DeclinioLeadSelectMenu.Add(new MotivoDeclinioLead { IdMotivoDeclinioLead = 2, Descricao = "Cliente vendeu o veículo", Ativo = true });
            DeclinioLeadSelectMenu.Add(new MotivoDeclinioLead { IdMotivoDeclinioLead = 3, Descricao = "Cliente não deseja mais plano de saúde", Ativo = true });
            DeclinioLeadSelectMenu.Add(new MotivoDeclinioLead { IdMotivoDeclinioLead = 4, Descricao = "Cliente possui benefício pela empresa onde trabalha", Ativo = true });
        }
        #endregion

        #region M�todos
        public async Task<IActionResult> OnGetAsync()
        {
            List<Task> initialTasks = new List<Task>();
            long idUsuarioLogado = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            DateTime dataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month - 7);
            DateTime dataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);
            DataInicialDefault = dataInicial.ToString("dd/MM/yyyy");
            DataFinalDefault = dataFinal.ToString("dd/MM/yyyy");
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            ListListEntityProposta = await PropostaModel.GetListCardPropostaAsync(idUsuarioLogado, new DateTime(2020, 07, 01), dataFinal, ListFaseProposta);
            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedMilliseconds / 1000.0);
            CarregarLists(idUsuarioLogado);
            
            return Page();
        }
        
        /*
        public IActionResult OnGet()
        {
            var stopwatch = new Stopwatch();
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            DateTime DataInicial = Util.GetFirstDayOfMonth(DateTime.Now.Month - 7);
            DateTime DataFinal = Util.GetLastDayOfMonth(DateTime.Now.Month);

            stopwatch.Start();
            ListListEntityProposta = PropostaModel.GetListListCardProposta(HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong(), DataInicial, DataFinal, "", "", 0, 0);
            stopwatch.Stop();
            Console.Write(stopwatch.ElapsedMilliseconds / 1000.0);

            CarregarLists();
            return Page();
        }
        */
        public IActionResult OnGetEdit(string statusId = null, string taskId = null, string declinioSelectId = null)
        {
            //IdDeclinioLeadTemp = declinioSelectId.ExtractByteOrNull();

            if ((!statusId.IsNullOrEmpty()) && (!taskId.IsNullOrEmpty()))
            {
                Proposta EntityProposta = PropostaModel.Get(taskId.ExtractLong());
                EntityProposta.IdFaseProposta = statusId.ExtractByteOrNull();
                // Instrução usada para atualizar o card, mas precisa atualizar o banco antes
                //EntityProposta.IdMotivoDeclinioLead = declinioSelectId.ExtractByteOrNull();
                PropostaModel.Update(EntityProposta);
            }

            return new JsonResult(new { status = true });
        }

        public async Task<IActionResult> OnPostPesquisaPropostasAsync(IFormCollection dados)
        {
            bool status = false;
            DateTime dataInicio = !string.IsNullOrEmpty(dados["dataInicio"]) ? Convert.ToDateTime(dados["dataInicio"]) : Util.GetFirstDayOfMonth(DateTime.Now.Month);
            DateTime dataFim = !string.IsNullOrEmpty(dados["dataFim"]) ? Convert.ToDateTime(dados["dataFim"]) : Util.GetFirstDayOfMonth(DateTime.Now.Month);
            ListFaseProposta = FasePropostaModel.GetListIdDescricao();
            byte.TryParse(dados["fase"], out byte fase);
            long idUsuario;
            List<FaseProposta> faseProposta = FasePropostaModel.GetListIdDescricao();

            if (!string.IsNullOrEmpty(dados["idCorretor"]) && !dados["idCorretor"].Equals("undefined"))
                idUsuario = long.Parse(dados["idCorretor"]);
            else if (!string.IsNullOrEmpty(dados["idSupervisor"]) && !dados["idSupervisor"].Equals("undefined"))
                idUsuario = long.Parse(dados["idSupervisor"]);
            else if (!string.IsNullOrEmpty(dados["idGerente"]) && !dados["idGerente"].Equals("undefined"))
                idUsuario = long.Parse(dados["idGerente"]);
            else
                idUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            ListListEntityProposta = await PropostaModel.GetListCardPropostaAsync(idUsuario, dataInicio, dataFim, ListFaseProposta, dados["operadora"]);

            if (ListListEntityProposta[0].Count > 0)
                status = true;

            return new JsonResult(new { status, fase, faseProposta, Propostas = ListListEntityProposta, periodoA = dataInicio.ToString(), periodoB = dataFim.ToString() });
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

        #region Filtros
        public IActionResult OnGetUsuariosSlave(string IdMaster)
        {
            List<UsuarioHierarquia> usuariosHierarquia = UsuarioHierarquiaModel.GetAllUsuarioSlave(IdMaster.ExtractLong());
            List<Usuario> usuariosSlave = UsuarioModel.GetAllUsuarioSlave(usuariosHierarquia);

            return new JsonResult(new { result = usuariosSlave });
        }
        #endregion

        public void CarregarLists(long idsuarioLogado)
        {
            byte? perfilUsuario = UsuarioModel.GetPerfil(idsuarioLogado);

            switch (perfilUsuario)
            {
                case (byte)(EnumeradorModel.Perfil.Administrador):
                    ListGerente = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Gerente);
                    break;
                case (byte)(EnumeradorModel.Perfil.Gerente):
                    UsuarioGerente = UsuarioModel.GetUsuarioGerente(idsuarioLogado);
                    break;
                case (byte)(EnumeradorModel.Perfil.Supervisor):
                    UsuarioSupervisor = UsuarioModel.GetUsuarioSupervisor(idsuarioLogado);
                    break;
                default:
                    break;
            }
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
