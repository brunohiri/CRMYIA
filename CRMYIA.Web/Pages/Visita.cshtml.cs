using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
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
        [BindProperty]
        public byte? IdPerfil { get; set; }

        #region Lists
        public List<StatusVisita> ListStatusVisita { get; set; }
        public Visita ResultadoVisita { get; set; }
        #endregion
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            long IdUsuario = GetIdUsuario();
            UsuarioPerfil EntityUsuarioPerfil = Business.PerfilModel.GetIdentificacaoPerfil(IdUsuario);
            IdPerfil = EntityUsuarioPerfil.IdPerfil;
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

            long IdUsuario = GetIdUsuario();

            ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Util.GetFirstDayOfMonth(DateTime.Now.Month).AddMonths(-3), Util.GetLastDayOfMonth(DateTime.Now.Month).AddMonths(3));

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }

        public JsonResult OnGetVisitasPesquisa(long IdPerfil, string Nome, string DataInicial, string DataFinal, int IdPerfilUsuario)
        {
            DateTime Inicio = Convert.ToDateTime(DataInicial);
            DateTime Fim = Convert.ToDateTime(DataFinal);
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = GetIdUsuario();
            if (Nome == "Selecione...")
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


        public IActionResult OnPostVisitas(EnviarCalendarioSazonalViewModel dados)
        {
            List<VisitaViewModel> ListVisita = null;
            CalendarioSazonal EntityCalendarioSazonal = null;

            //var Data = dados.DataInicioFim.Split('-');
            //DateTime DataInicio = Convert.ToDateTime(Data[0]);
            //DateTime DataFim = Convert.ToDateTime(Data[1]);

            long IdUsuario = GetIdUsuario();

            if(dados.Repete == 7 && dados.Frequencia == 1 && dados.Repetir > 0 && (dados.Termina > 0))
            {
                PersonalizadoDiariamente(dados);
            }

            //Business.CalendarioSazonalModel.Add(ListCalendarioSazonal1);
            //Business.VisitaModel.AddList(ListVisita);

            //if (dados.Descricao.IsNullOrEmpty())
            //   return new JsonResult(new { status = false, mensagem = "Campo Título obrigatório em branco!" });
            //else if (dados.DataSazonal == null)
            //    return new JsonResult(new { status = false, mensagem = "Campo Data obrigatório em branco!" });
            //else
            //{
            //    if (dados.IdCalendarioSazonal == 0)
            //    {
            //        if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
            //        {
            //            Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
            //            {
            //                Descricao = dados.Descricao,
            //                Cor = dados.Cor,
            //                Tipo = dados.Tipo,
            //                DataSazonal = dados.DataSazonal,
            //                DataInicio = dados.DataInicio,
            //                DataFim = dados.DataFim,
            //                DataCadastro = DateTime.Now,
            //                ExisteCampanha = dados.ExisteCampanha,
            //                Ativo = dados.Ativo
            //            });

            //            EntityCalendarioSazonal = CalendarioSazonalModel.GetLastId();

            //            Business.VisitaModel.Add(new Visita()
            //            {
            //                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal,
            //                IdCalendarioSazonal = EntityCalendarioSazonal.IdCalendarioSazonal,
            //                Descricao = EntityCalendarioSazonal.Descricao,
            //                DataAgendamento = EntityCalendarioSazonal.DataSazonal,
            //                DataInicio = EntityCalendarioSazonal.DataInicio,
            //                DataFim = EntityCalendarioSazonal.DataFim,
            //                Visivel = (byte)1,
            //                Cor = EntityCalendarioSazonal.Cor,
            //                DataCadastro = DateTime.Now,
            //                IdUsuario = IdUsuario
            //            });
            //        }
            //        else if (dados.Tipo == 3)
            //        {
            //            UsuarioPerfil EntityUsuarioPerfil =  PerfilModel.GetIdentificacaoPerfil(IdUsuario);
            //            byte? Visivel = 0;
            //            if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Administrador)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Administrador;
            //            }
            //            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Gerente)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Gerente;
            //            }
            //            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Supervisor)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Supervisor;
            //            }
            //            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Corretor)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Corretor;
            //            }
            //            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Vendedor)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Vendedor;
            //            }
            //            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Marketing)
            //            {
            //                Visivel = (byte)EnumeradorModel.Visualizacao.Marketing;
            //            }

            //            if (dados.DataInicio != null && dados.DataFim != null)
            //            {
            //                Business.VisitaModel.Add(new Visita()
            //                {
            //                    Descricao = dados.Descricao,
            //                    DataAgendamento = dados.DataSazonal,
            //                    DataInicio = dados.DataInicio,
            //                    DataFim =dados.DataFim,
            //                    DataCadastro = DateTime.Now,
            //                    Observacao = dados.Observacao,
            //                    IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
            //                    Visivel = Visivel,
            //                    Cor = dados.Cor,
            //                    IdUsuario = IdUsuario
            //                });
            //            }
            //            else
            //            {
            //                Business.VisitaModel.Add(new Visita()
            //                {
            //                    Descricao = dados.Descricao,
            //                    DataAgendamento = dados.DataSazonal,
            //                    DataCadastro = DateTime.Now,
            //                    Observacao = dados.Observacao,
            //                    IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
            //                    Visivel = Visivel,
            //                    Cor = dados.Cor,
            //                    IdUsuario = IdUsuario
            //                });
            //            }

            //            //Notificação
            //            UsuarioHierarquia EntityUsuarioHierarquia = UsuarioHierarquiaModel.GetSlave(IdUsuario);
            //            if (EntityUsuarioHierarquia != null)
            //            {
            //                Visita EntityVisita = Business.VisitaModel.GetLastId();
            //                Notificacao EntityNotificacao = NotificacaoModel.Add(new Notificacao()
            //                {
            //                    IdUsuarioCadastro = IdUsuario,
            //                    IdUsuarioVisualizar = EntityUsuarioHierarquia.IdUsuarioMaster,
            //                    Titulo = null,
            //                    Descricao = dados.Descricao,
            //                    Url = "/Visita?Id=" + HttpUtility.UrlEncode(Criptography.Encrypt(EntityVisita.IdVisita.ToString())),
            //                    Visualizado = false,
            //                    DataCadastro = DateTime.Now,
            //                    Ativo = true
            //                });
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //UpDate

            //        //Visita EntityVisita = Business.VisitaModel.Get(Entity.IdVisita);
            //        //EntityVisita.Descricao = Entity.Descricao;
            //        //EntityVisita.DataAgendamento = Entity.DataAgendamento;
            //        //EntityVisita.DataCadastro = DateTime.Now;
            //        //EntityVisita.Observacao = Entity.Observacao;
            //        //EntityVisita.IdStatusVisitaNavigation = null;
            //        //EntityVisita.IdStatusVisita = (Entity.IdStatusVisita == null ? (byte)EnumeradorModel.StatusVisita.Agendada : Entity.IdStatusVisita);
            //        //Business.VisitaModel.Update(EntityVisita);
            //    }

            //    ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Util.GetFirstDayOfMonth(DateTime.Now.Month).AddMonths(-3), Util.GetLastDayOfMonth(DateTime.Now.Month).AddMonths(3));

               return new JsonResult(new { status = true, listVisita = ListVisita });
            //}
        }
        public IActionResult OnGetTodosPerfil()
        {
            byte? IdPerfil = 0;
            long IdUsuario = GetIdUsuario();
            List<PerfilViewModel> ListEntity = null;
            
            ListEntity =  PerfilModel.GetListIdDescricao(IdUsuario, ref IdPerfil);

            return new JsonResult(new { status = true, listVisita = ListEntity, IdPerfil = IdPerfil });
        }

        public IActionResult OnGetTodosNomes()
        {
            long IdUsuario = GetIdUsuario();
            List<Usuario> ListEntity = null;

            ListEntity = UsuarioModel.GetListIdNome(IdUsuario);

            return new JsonResult(new { status = true, listVisita = ListEntity });
        }

        public IActionResult OnGetDesativarNotificacao(long IdNotificacao)
        {
            bool status;
            if (IdNotificacao > 0)
            {
                NotificacaoModel.DesativarNotificacao(IdNotificacao);
                status = true; 
            }
            else
            {
                status = false;
            }
            return new JsonResult(new {status = status });
        }

        public IActionResult OnGetBuscarVisita(string Id = null, string IdNotificacao = null)
        {
            bool status = false;
            VisitaViewModel Resultado = null;
            ResultadoVisita = Business.VisitaModel.GetVisitaId(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            if (ResultadoVisita != null)
            {
                Resultado = (new VisitaViewModel()
                {
                    sourceId = ResultadoVisita.IdVisita,
                    backgroundColor = ResultadoVisita.IdStatusVisitaNavigation.CorHexa,
                    borderColor = ResultadoVisita.IdStatusVisitaNavigation.CorHexa,
                    start = ResultadoVisita.DataAgendamento.Value,
                    title = ResultadoVisita.Descricao,
                    allDay = false
                });
                status = true;
                if (IdNotificacao != null)
                {
                    NotificacaoModel.DesativarNotificacao(Criptography.Decrypt(HttpUtility.UrlDecode(IdNotificacao)).ExtractLong());
                }
            }           
            return new JsonResult(new { status = status, listVisita = Resultado });
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

        private void PersonalizadoDiariamente(EnviarCalendarioSazonalViewModel dados)
        {
            long IdUsuario = GetIdUsuario();

            UsuarioPerfil EntityUsuarioPerfil = PerfilModel.GetIdentificacaoPerfil(IdUsuario);
            byte? Visivel = 0;
            if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Administrador)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Administrador;
            }
            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Gerente)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Gerente;
            }
            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Supervisor)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Supervisor;
            }
            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Corretor)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Corretor;
            }
            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Vendedor)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Vendedor;
            }
            else if (EntityUsuarioPerfil.IdPerfil == (byte)EnumeradorModel.Perfil.Marketing)
            {
                Visivel = (byte)EnumeradorModel.Visualizacao.Marketing;
            }

            //#

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;
            int i = 0;
            List<DateTime> Dia = new List<DateTime>();
            int meses = 0;

            int AnoInicio = DataInicio.Year;
            int MesInicio = DataInicio.Month;

            int AnoFim = DataFim.Year;
            int MesFim = DataFim.Month;

            if (dados.Termina == 1)
            {
                Business.VisitaModel.Add(new Visita()
                {
                    Descricao = dados.Descricao,
                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario
                });
            }

            int j = 0;

            //#

            if (dados.Termina == 1)
            {
                DataInicio = new DateTime(AnoInicio, MesInicio, DateTime.DaysInMonth(AnoInicio, MesInicio), DataInicio.Hour, DataInicio.Minute, DataInicio.Second);

                DataFim = new DateTime(AnoFim, MesFim, DateTime.DaysInMonth(AnoFim, MesFim), DataFim.Hour, DataFim.Minute, DataFim.Second);

                do
                {
                    DataInicio = DataInicio.AddDays(dados.Repetir);
                    Dia.Add(DataInicio);

                    DataFim = DataFim.AddDays(dados.Repetir);
                    Dia.Add(DataFim);
                    Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));

                    Business.VisitaModel.Add(new Visita()
                    {
                        Descricao = dados.Descricao,
                        DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario
                    });

                    j++;

                } while (j < ((365 * 8) / dados.Repetir));

            }else if (dados.Termina == 2)
            {
                

                DataInicio = new DateTime(AnoInicio, MesInicio, DateTime.DaysInMonth(AnoInicio, MesInicio), DataInicio.Hour, DataInicio.Minute, DataInicio.Second);

                DataFim = new DateTime(AnoFim, MesFim, DateTime.DaysInMonth(AnoFim, MesFim), DataFim.Hour, DataFim.Minute, DataFim.Second);
                //var a = (DataFim - DataInicio).Days;

                while (DataInicio <= dados.DataTerminaEm){
                    
                    Dia.Add(DataInicio);

                    DataFim = DataFim.AddDays(dados.Repetir);
                    Dia.Add(DataFim);
                    //Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));

                    Business.VisitaModel.Add(new Visita()
                    {
                        Descricao = dados.Descricao,
                        DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario
                    });

                    DataInicio = DataInicio.AddDays(dados.Repetir);
                    j++;

                }
            //} while (new DateTime(DataFim.Year, DataFim.Month, DataFim.Day) != new DateTime(dados.DataTerminaEm.Year, dados.DataTerminaEm.Month, dados.DataTerminaEm.Day)) ;
        }
        }
    }


        #endregion
}

