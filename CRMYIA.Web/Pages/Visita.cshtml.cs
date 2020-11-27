using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using CRMYIA.Web.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class VisitaModel : PageModel
    {
        #region Propriedades

        [BindProperty]
        public Visita Entity { get; set; }

        #region Lists
        public List<StatusVisita> ListStatusVisita { get; set; }
        #endregion
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet()
        { 
                Entity = new Visita();
                ListStatusVisita = StatusVisitaModel.GetList();
                return Page();
        }

        public IActionResult OnGetByIdVisita(string IdVisita = null)
        {
            if (!IdVisita.IsNullOrEmpty())
            {
                Visita EntityVisita = Business.VisitaModel.Get(IdVisita.ExtractLong());
                return new JsonResult(new { status = true, entityVisita = EntityVisita });
            }
            else
                return new JsonResult(new { status = false });
        }

        public IActionResult OnGetVisitas()
        {
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Util.GetFirstDayOfMonth(DateTime.Now.Month).AddMonths(-3), Util.GetLastDayOfMonth(DateTime.Now.Month).AddMonths(3));

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }

        public JsonResult OnGetVisitasPesquisa(long IdPerfil, string Nome, string DataInicial, string DataFinal, int IdPerfilUsuario)
        {
            DateTime Inicio = Convert.ToDateTime(DataInicial);
            DateTime Fim = Convert.ToDateTime(DataFinal);
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            if(Nome == "Selecione um Nome")
            {
                Nome = User.Identity.Name;
            }
            if (Nome == null)
            {
                Nome = User.Identity.Name;
            }
            ListVisita = Business.VisitaModel.GetListByDataAgendamentoPesquisaViewModel(Inicio, Fim, IdPerfil, Nome, IdPerfilUsuario);

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }


        public IActionResult OnPostVisitas()
        {
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            if (Entity.Descricao.IsNullOrEmpty())
                return new JsonResult(new { status = false, mensagem = "Campo Título obrigatório em branco!" });
            else
                 if (Entity.DataAgendamento == null)
                return new JsonResult(new { status = false, mensagem = "Campo Data Hora obrigatório em branco!" });
            else
            {
                if (Entity.IdVisita == 0)
                {
                    Business.VisitaModel.Add(new Visita()
                    {
                        Descricao = Entity.Descricao,
                        DataAgendamento = Entity.DataAgendamento,
                        DataCadastro = DateTime.Now,
                        Observacao = Entity.Observacao,
                        IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                        IdUsuario = IdUsuario
                    });

                    UsuarioHierarquia EntityUsuarioHierarquia = UsuarioHierarquiaModel.GetSlave(IdUsuario);
                    Notificacao EntityNotificacao = NotificacaoModel.Add(new Notificacao()
                    {
                        IdUsuarioCadastro = Entity.IdUsuario,
                        IdUsuarioVisualizar = EntityUsuarioHierarquia.IdUsuarioMaster,
                        Titulo = null,
                        Descricao = "Novo Agendamento",
                        Url = "",
                        Visualizado = false,
                        DataCadastro = DateTime.Now,
                        Ativo = true
                    });

                    //NotificacaoController NcCtrl = new NotificacaoController();
                    //NcCtrl.NotificacaoHub(EntityNotificacao);

                }
                else
                {
                    Visita EntityVisita = Business.VisitaModel.Get(Entity.IdVisita);
                    EntityVisita.Descricao = Entity.Descricao;
                    EntityVisita.DataAgendamento = Entity.DataAgendamento;
                    EntityVisita.DataCadastro = DateTime.Now;
                    EntityVisita.Observacao = Entity.Observacao;
                    EntityVisita.IdStatusVisitaNavigation = null;
                    EntityVisita.IdStatusVisita = (Entity.IdStatusVisita == null ? (byte)EnumeradorModel.StatusVisita.Agendada : Entity.IdStatusVisita);
                    Business.VisitaModel.Update(EntityVisita);
                }

                ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Util.GetFirstDayOfMonth(DateTime.Now.Month).AddMonths(-3), Util.GetLastDayOfMonth(DateTime.Now.Month).AddMonths(3));

                return new JsonResult(new { status = true, listVisita = ListVisita });
            }
        }
        public IActionResult OnGetTodosPerfil()
        {
            byte? IdPerfil = 0;
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            List<PerfilViewModel> ListEntity = null;
            
            ListEntity =  PerfilModel.GetListIdDescricao(IdUsuario, ref IdPerfil);

            return new JsonResult(new { status = true, listVisita = ListEntity, IdPerfil = IdPerfil });
        }

        public IActionResult OnGetTodosNomes()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            List<Usuario> ListEntity = null;

            ListEntity = UsuarioModel.GetListIdNome(IdUsuario);

            return new JsonResult(new { status = true, listVisita = ListEntity });
        }
    }

        #endregion
}

