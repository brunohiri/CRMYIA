using System;
using System.Collections.Generic;
using System.Globalization;
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

        [BindProperty]
        public string MesDiaDaSemana { get; set; }

        [BindProperty]
        public int MesDataColocacao { get; set; }

        #region Lists
        public List<StatusVisita> ListStatusVisita { get; set; }
        public Visita ResultadoVisita { get; set; }
        #endregion
        #endregion

        #region Construtores
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            long IdUsuario = GetIdUsuario();
            UsuarioPerfil EntityUsuarioPerfil = Business.PerfilModel.GetIdentificacaoPerfil(IdUsuario);
            IdPerfil = EntityUsuarioPerfil.IdPerfil;
            Entity = new Visita();
            ListStatusVisita = StatusVisitaModel.GetList();
       
            for (DateTime Data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01); Data <= DateTime.Today; Data = Data.AddDays(1))
            {
                MesDiaDaSemana = Data.DayOfWeek.ToString();
                MesDataColocacao = Util.ObterClassificacao(Data);
                //var a = Data.ToShortDateString() + " - " + Util.ObterClassificacao(Data) + " - " + Data.DayOfWeek;
            }
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

        public IActionResult OnPostObterVisitas(DateTime Inicio, DateTime Fim)
        {
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = GetIdUsuario();

            ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, Inicio, Fim);

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }

        public IActionResult OnPostObter(Visita Entity)
        {
            Visita EntityVisita = null;
            bool ExisteMaisQueUm = false;
            bool status = false;

            EntityVisita = Business.VisitaModel.Get(Entity.IdVisita);
            ExisteMaisQueUm = Business.VisitaModel.ExisteMaisQueUm(EntityVisita.GuidId);
            if(EntityVisita != null)
            {
                status = true;
            }

            return new JsonResult(new { status = status, entityVisita = EntityVisita, existeMaisQueUm = ExisteMaisQueUm });
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

        public IActionResult OnPostDataColocacao(string DataInicio)
        {
            bool status = false;
            DateTime Data = new DateTime(Convert.ToDateTime(DataInicio).Year, Convert.ToDateTime(DataInicio).Month, Convert.ToDateTime(DataInicio).Day);
            MesDiaDaSemana = Data.DayOfWeek.ToString();
            MesDataColocacao = Util.ObterClassificacao(Data);
            if(MesDiaDaSemana != null && MesDataColocacao > 0)
            {
                status = true;
            }
            return new JsonResult(new { status = status, mesDiaDaSemana = MesDiaDaSemana, mesDataColocacao = MesDataColocacao, dia = Data.Day });
        }
        public IActionResult OnPostVisitas(EnviarCalendarioSazonalViewModel dados)
        {
            List<VisitaViewModel> ListVisita = null;

            long IdUsuario = GetIdUsuario();
                if (dados.Repete == 1)//Nunca
                {
                    Nunca(dados);
                }
                else if (dados.Repete == 2)//Todos os Dias
                {
                    TodosOsDias(dados);
                }
                else if (dados.Repete == 3)//A Cada Semana
                {
                    CadaSemana(dados);
                }
                else if (dados.Repete == 4)//A Cada 2 Semanas
                {
                    CadaDuasSemana(dados);
                }
                else if (dados.Repete == 5)//A Cada M�s
                {
                    CadaMes(dados);
                }
                else if (dados.Repete == 6)//A Cada Ano
                {
                    CadaAno(dados);
                }
                else if (dados.Repete == 7 && dados.Frequencia == 1 && dados.Repetir > 0 && (dados.Termina > 0))//Diariamente
                {
                    PersonalizadoDiariamente(dados);
                }
                else if (dados.Repete == 7 && dados.Frequencia == 2 && dados.Repetir > 0 && (dados.Termina > 0))//Semanalmente
                {
                    PersonalizadoSemanalmente(dados);
                }
                else if (dados.Repete == 7 && dados.Frequencia == 3 && dados.Repetir > 0 && (dados.Termina > 0))//Mensalmente
                {
                    PersonalizadoMensalmente(dados);
                }
                else if (dados.Repete == 7 && dados.Frequencia == 4 && dados.Repetir > 0 && (dados.Termina > 0))//Mensalmente
                {
                    PersonalizadoAnualmente(dados);
                }

                if(dados.Tipo == 3)
                {
                    GeraNotificacao(dados);
                }
                ListVisita = Business.VisitaModel.GetListByDataAgendamentoReturnsViewModel(IdUsuario, dados.StartStr, dados.EndStr);

            return new JsonResult(new { status = true, listVisita = ListVisita });
        }
        public IActionResult OnPostExcluir(EnviarCalendarioSazonalViewModel dados)
        {
            bool status = false;
            try
            {
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Remove(Business.VisitaModel.Get(dados.IdVisita));
                    status = true;
                }
                else if (dados.OpExcluirAlterar == 2)
                {
                    Business.VisitaModel.RemoveEventosSeguintes(dados.IdVisita, dados.GuidId);
                    status = true;
                }
                else if (dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.RemoveTodosEventos(dados.GuidId);
                    status = true;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return new JsonResult(new { status = status });
            
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
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;
            int i = 0;
            List<DateTime> Dia = new List<DateTime>();
            int meses = 0;

            int AnoInicio = DataInicio.Year;
            int MesInicio = DataInicio.Month;

            int AnoFim = DataFim.Year;
            int MesFim = DataFim.Month;

            int j = 0;

            if (dados.Termina == 1)
            {
                DataInicio = new DateTime(AnoInicio, MesInicio, DateTime.DaysInMonth(AnoInicio, MesInicio), DataInicio.Hour, DataInicio.Minute, DataInicio.Second);

                DataFim = new DateTime(AnoFim, MesFim, DateTime.DaysInMonth(AnoFim, MesFim), DataFim.Hour, DataFim.Minute, DataFim.Second);

                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    do
                    {
                        if (dados.IdVisita == 0)
                        {
                            Business.VisitaModel.Add(new Visita()
                            {
                                Descricao = dados.Descricao,
                                DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                DataInicio = DataInicio,
                                DataFim = DataFim,
                                DataCadastro = DateTime.Now,
                                Observacao = dados.Observacao,
                                IdStatusVisita = IdStatusVisita,
                                Visivel = Visivel,
                                Tipo = dados.Tipo,
                                GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                Termina = dados.Termina
                            });
                        }
                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                        {
                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                            {
                                Descricao = dados.Descricao,
                                DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                DataInicio = DataInicio,
                                DataFim = DataFim,
                                DataCadastro = DateTime.Now,
                                Observacao = dados.Observacao,
                                IdStatusVisita = IdStatusVisita,
                                Visivel = Visivel,
                                Tipo = dados.Tipo,
                                 GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                Termina = dados.Termina
                            }, dados.IdVisita, dados.GuidId);
                        }
                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                        {
                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                            {
                                Descricao = dados.Descricao,
                                DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                DataInicio = DataInicio,
                                DataFim = DataFim,
                                DataCadastro = DateTime.Now,
                                Observacao = dados.Observacao,
                                IdStatusVisita = IdStatusVisita,
                                Visivel = Visivel,
                                Tipo = dados.Tipo,
                                 GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                Termina = dados.Termina
                            }, dados.GuidId);
                        }

                        DataInicio = DataInicio.AddDays(dados.Repetir);
                        Dia.Add(DataInicio);

                        DataFim = DataFim.AddDays(dados.Repetir);
                        Dia.Add(DataFim);
                        Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));

                        j++;

                    } while (j < 730);
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }

            }else if (dados.Termina == 2)
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        DataTerminaEm = dados.DataTerminaEm
                    });
                }
                else
                {
                    while (new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day) <= dados.DataTerminaEm)
                    {
                        //Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));
                        if(dados.IdVisita == 0)
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
                                GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                DataTerminaEm = dados.DataTerminaEm,
                                Termina = dados.Termina
                            }); 
                        }
                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                        {
                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                            {
                                Descricao = dados.Descricao,
                                DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                DataInicio = DataInicio,
                                DataFim = DataFim,
                                DataCadastro = DateTime.Now,
                                Observacao = dados.Observacao,
                                IdStatusVisita = IdStatusVisita,
                                Visivel = Visivel,
                                Tipo = dados.Tipo,
                                 GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                DataTerminaEm = dados.DataTerminaEm,
                                Termina = dados.Termina
                            }, dados.IdVisita, dados.GuidId);
                        }
                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                        {
                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                            {
                                Descricao = dados.Descricao,
                                DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                DataInicio = DataInicio,
                                DataFim = DataFim,
                                DataCadastro = DateTime.Now,
                                Observacao = dados.Observacao,
                                IdStatusVisita = IdStatusVisita,
                                Visivel = Visivel,
                                Tipo = dados.Tipo,
                                 GuidId = GuidId.ToString(),
                                Cor = dados.Cor,
                                IdUsuario = IdUsuario,
                                Repete = dados.Repete,
                                Frequencia = dados.Frequencia,
                                Repetir = dados.Repetir,
                                DataTerminaEm = dados.DataTerminaEm,
                                Termina = dados.Termina
                            }, dados.GuidId);
                        }
                        Dia.Add(DataInicio);
                        Dia.Add(DataFim);
                        DataFim = DataFim.AddDays(dados.Repetir);
                        DataInicio = DataInicio.AddDays(dados.Repetir);
                        j++;

                    }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            //} while (new DateTime(DataFim.Year, DataFim.Month, DataFim.Day) != new DateTime(dados.DataTerminaEm.Year, dados.DataTerminaEm.Month, dados.DataTerminaEm.Day)) ;
            }
        }
    
        private void PersonalizadoSemanalmente(EnviarCalendarioSazonalViewModel dados)
        {
            var Semana = dados.Semana.Split(',');

            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;
            int i = 0;
            List<DateTime> Dia = new List<DateTime>();
            int meses = 0;

            //int AnoInicio = DataInicio.Year;
            //int MesInicio = DataInicio.Month;

            //int AnoFim = DataFim.Year;
            //int MesFim = DataFim.Month;

            var DataInicioYear = DataInicio.Year + 8;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 8;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            int j = 0;

            if (dados.Termina == 1)
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                        GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        Semana = dados.Semana,
                        MesDataColocacao = dados.MesDataColocacao,
                        MesDiaDaSemana = dados.MesDiaDaSemana,
                        MesDia = dados.MesDia
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    do
                    {
                        var a = DataInicio.DayOfWeek.ToString();
                        var e = Array.Exists(Semana, element => element.StartsWith(DataInicio.DayOfWeek.ToString()));
                        if (Array.Exists(Semana, element => element.StartsWith(DataInicio.DayOfWeek.ToString())) == true)
                        {

                            if (dados.IdVisita == 0)
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
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                });
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = DataInicio,
                                    DataFim = DataFim,
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                }, dados.IdVisita, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = DataInicio,
                                    DataFim = DataFim,
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                }, dados.GuidId);
                            }

                            j++;
                        }
                        DataInicio = DataInicio.AddDays(1);
                        Dia.Add(DataInicio);

                        DataFim = DataFim.AddDays(1);
                        Dia.Add(DataFim);
                        Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));

                    } while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond));
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }else if (dados.Termina == 2)
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                        GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        DataTerminaEm = dados.DataTerminaEm,
                        Semana = dados.Semana,
                        MesDataColocacao = dados.MesDataColocacao,
                        MesDiaDaSemana = dados.MesDiaDaSemana,
                        MesDia = dados.MesDia
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    while (DataInicio <= dados.DataTerminaEm)
                    {
                        //Dia.Add(new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day));
                        var a = DataInicio.DayOfWeek.ToString();
                        var e = Array.Exists(Semana, element => element.StartsWith(DataInicio.DayOfWeek.ToString()));
                        if (Array.Exists(Semana, element => element.StartsWith(DataInicio.DayOfWeek.ToString())) == true)
                        {
                            if (dados.IdVisita == 0)
                            {
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = DataInicio,
                                    DataFim = DataFim,
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    DataTerminaEm = dados.DataTerminaEm,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                });
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = DataInicio,
                                    DataFim = DataFim,
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    DataTerminaEm = dados.DataTerminaEm,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                }, dados.IdVisita, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = DataInicio,
                                    DataFim = DataFim,
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                    Repetir = dados.Repetir,
                                    Termina = dados.Termina,
                                    DataTerminaEm = dados.DataTerminaEm,
                                    Semana = dados.Semana,
                                    MesDataColocacao = dados.MesDataColocacao,
                                    MesDiaDaSemana = dados.MesDiaDaSemana,
                                    MesDia = dados.MesDia
                                }, dados.GuidId);
                            }
                        }
                        Dia.Add(DataInicio);

                        DataFim = DataFim.AddDays(1);
                        Dia.Add(DataFim);

                        DataInicio = DataInicio.AddDays(1);
                        j++;
                    }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }
        }

        private void PersonalizadoMensalmente(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;
            int i = 0;
            List<DateTime> Dia = new List<DateTime>();
            int meses = 0;

            int AnoInicio = DataInicio.Year;
            int MesInicio = DataInicio.Month;

            int AnoFim = DataFim.Year;
            int MesFim = DataFim.Month;

            DateTime mes = new DateTime(DataInicio.Year, DataInicio.Month, DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month));
            DateTime UltimaDataDoMes;

            var DataInicioYear = DataInicio.Year + 10;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 8;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            var DayOfWeek = DataInicio.DayOfWeek.ToString();

            //Usa o calend�rio padr�o do InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            if (dados.Termina == 1)//Terminar Nunca
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        DataTerminaEm = dados.DataTerminaEm,
                        MesDia = dados.MesDia,
                        MesDataColocacao = dados.MesDataColocacao,
                        MesDiaDaSemana = dados.MesDiaDaSemana,
                        SelectMensalmente = dados.SelectMensalmente
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond))
                    {
                        if (dados.SelectMensalmente == 1)//Mensalmente no dia X
                        {
                            if (dados.MesDia == DataInicio.Day)
                            {
                                if (DataInicio.Day == 31 && dados.MesDia == DataInicio.Day)//Dia 31
                                {
                                    if (DataInicio.Day == 31 && DataFim.Day == 31)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 31

                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 31 && DataFim.Day == 30)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 31 && DataFim.Day == 29)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 31 && DataFim.Day == 28)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 31 && DataFim.Day < 28)
                                    {
                                        //DataInicio igual a 31 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                    DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                                }
                                else if (DataInicio.Day == 30 && dados.MesDia == DataInicio.Day)//Dia 30
                                {
                                    if (DataInicio.Day == 30 && DataFim.Day == 30)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 30 && DataFim.Day == 29)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 30 && DataFim.Day == 28)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 30 && DataFim.Day < 28)
                                    {
                                        //DataInicio igual a 30 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                    DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                                }
                                else if (DataInicio.Day == 29 && dados.MesDia == DataInicio.Day)//Dia 29
                                {
                                    var a = DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month);
                                    if (DataInicio.Day == 29 && DataFim.Day == 28)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 29 && DataFim.Day == 29)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 29 && DataFim.Day == 30)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 29 && DataFim.Day == 31)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 31
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    else if (DataInicio.Day == 29 && DataFim.Day < 28)
                                    {
                                        //DataInicio igual a 29 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                    }
                                    DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                    DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                                }
                                else if (DataInicio.Day <= 28 && DataInicio.Day <= 28 && dados.MesDia == DataInicio.Day)//Dia 29
                                {
                                    //DataInicio menor igual 28 e DataFim menor igual 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                    DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                    DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                                }
                            }
                            DataInicio = DataInicio.AddDays(1);//Adicona um Dia 
                            DataFim = DataFim.AddDays(1);//Adicona um Dia 
                            if (!DateTime.IsLeapYear(DataInicio.Year) && DataInicio.Month == 2 && DataInicio.Day == 28 && dados.MesDia == 29)//anos que n�o s�o bissexto, no m�s de fevereiro e no dia 28
                            {
                                DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                            }
                        }


                        if (dados.SelectMensalmente == 2)//Se for selecionado a op��o 2, buscando dia da semana e coloca��o do dia.
                        {
                            DayOfWeek = DataInicio.DayOfWeek.ToString();
                            if (dados.MesDataColocacao == Util.ObterClassificacao(DataInicio) && dados.MesDiaDaSemana == DayOfWeek)
                            {

                                if (DataInicio.Day == 31)//Dia 31
                                {
                                    if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 31
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                    {
                                        //DataInicio igual a 31 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                    {
                                        //DataInicio igual a 31 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                }
                                else if (DataInicio.Day == 30)//Dia 30
                                {
                                    if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                    {
                                        //DataInicio igual a 30 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                    {
                                        //DataInicio igual a 30 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }

                                }
                                else if (DataInicio.Day == 29)//Dia 29
                                {
                                    if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 29
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 30
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                    {
                                        //DataInicio igual a 29 e DataFim igual a 31
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                    else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                    {
                                        //DataInicio igual a 29 e DataFim menor que 28
                                        if (dados.IdVisita == 0)
                                        {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                        {
                                            Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.IdVisita, dados.GuidId);
                                        }
                                        else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                        {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                        }
                                        DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                        DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                        DataInicio = myCal.AddDays(DataInicio, -16);
                                        DataFim = myCal.AddDays(DataFim, -16);
                                    }
                                }
                                else if (DataInicio.Day <= 28 && DataFim.Day <= 28)//Dia 28
                                {
                                    //DataInicio menor igual 28 e DataFim menor igual 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                            }
                            DataInicio = myCal.AddDays(DataInicio, 1);//Adicona uma Semana 
                            DataFim = myCal.AddDays(DataFim, 1); ;//Adicona uma Semana 
                        }

                    }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }
            else if (dados.Termina == 2)//Termina na Data
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        DataTerminaEm = dados.DataTerminaEm,
                        MesDia = dados.MesDia,
                        MesDataColocacao = dados.MesDataColocacao,
                        MesDiaDaSemana = dados.MesDiaDaSemana,
                        SelectMensalmente = dados.SelectMensalmente
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else 
                { 
                    while (dados.DataTerminaEm >= new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day))
                {
                    if (dados.SelectMensalmente == 1)//Mensalmente no dia X
                    {
                        if (dados.MesDia == DataInicioDay)
                        {
                            if (DataInicioDay == 31)//Dia 31
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 31 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                            }
                            else if (DataInicioDay == 30)//Dia 30
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 30 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }

                            }
                            else if (DataInicioDay == 29)//Dia 29
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                            Business.VisitaModel.Add(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                            Business.VisitaModel.UpdateTodosEventos(new Visita()
                                            {
                                                Descricao = dados.Descricao,
                                                DataAgendamento = DateTime.Now,
                                                DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                                DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                                DataCadastro = DateTime.Now,
                                                Observacao = dados.Observacao,
                                                IdStatusVisita = IdStatusVisita,
                                                Visivel = Visivel,
                                                Tipo = dados.Tipo,
                                                 GuidId = GuidId.ToString(),
                                                Cor = dados.Cor,
                                                IdUsuario = IdUsuario,
                                                Repete = dados.Repete,
                                                Frequencia = dados.Frequencia,
                                                Repetir = dados.Repetir,
                                                Termina = dados.Termina,
                                                DataTerminaEm = dados.DataTerminaEm,
                                                MesDia = dados.MesDia,
                                                MesDataColocacao = dados.MesDataColocacao,
                                                MesDiaDaSemana = dados.MesDiaDaSemana,
                                                SelectMensalmente = dados.SelectMensalmente
                                            }, dados.GuidId);
                                    }
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 29 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                }
                            }
                            else if (DataInicioDay <= 28 && DataFimDay <= 28)//Dia 29
                            {
                                //DataInicio menor igual 28 e DataFim menor igual 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicioDay, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicioDay, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicioDay, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    }, dados.GuidId);
                                }
                            }
                        }
                        DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                        DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                    }


                    if (dados.SelectMensalmente == 2)//Se for selecionado a op��o 2, buscando dia da semana e coloca��o do dia.
                    {
                        DayOfWeek = DataInicio.DayOfWeek.ToString();
                        if (dados.MesDataColocacao == Util.ObterClassificacao(DataInicio) && dados.MesDiaDaSemana == DayOfWeek)
                        {

                            if (DataInicio.Day == 31)//Dia 31
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 31 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                            }
                            else if (DataInicio.Day == 30)//Dia 30
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 30 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }

                            }
                            else if (DataInicio.Day == 29)//Dia 29
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 29 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                            Repetir = dados.Repetir,
                                            Termina = dados.Termina,
                                            DataTerminaEm = dados.DataTerminaEm,
                                            MesDia = dados.MesDia,
                                            MesDataColocacao = dados.MesDataColocacao,
                                            MesDiaDaSemana = dados.MesDiaDaSemana,
                                            SelectMensalmente = dados.SelectMensalmente
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                            }
                            else if (DataInicio.Day <= 28 && DataFim.Day <= 28)//Dia 28
                            {
                                //DataInicio menor igual 28 e DataFim menor igual 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm,
                                        MesDia = dados.MesDia,
                                        MesDataColocacao = dados.MesDataColocacao,
                                        MesDiaDaSemana = dados.MesDiaDaSemana,
                                        SelectMensalmente = dados.SelectMensalmente
                                    }, dados.GuidId);
                                }
                                DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                            }
                        }
                        DataInicio = myCal.AddDays(DataInicio, 1);//Adicona uma Semana 
                        DataFim = myCal.AddDays(DataFim, 1); ;//Adicona uma Semana 
                    }

                }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }
        }

        private void PersonalizadoAnualmente(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;
            int i = 0;
            List<DateTime> Dia = new List<DateTime>();
            int meses = 0;

            int AnoInicio = DataInicio.Year;
            int MesInicio = DataInicio.Month;

            int AnoFim = DataFim.Year;
            int MesFim = DataFim.Month;

            DateTime mes = new DateTime(DataInicio.Year, DataInicio.Month, DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month));
            DateTime UltimaDataDoMes;

            var DataInicioYear = DataInicio.Year + 10;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 8;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            var DayOfWeek = DataInicio.DayOfWeek.ToString();

            //Usa o calend�rio padr�o do InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            if (dados.Termina == 1)//Terminar Nunca
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        DataTerminaEm = dados.DataTerminaEm
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond))
                    {
                        if (DateTime.IsLeapYear(DataInicio.Year))
                        {
                            if (DataInicioDay == 29)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicioDay == 29 && DataFimDay >= 28)
                            {
                                //DataInicio igual a 29 e DataFim menor igual que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicioDay == 29 && DataFimDay < 28)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicioDay <= 28 && DataFimDay <= 28)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicioDay >= 30 && DataFimDay >= 30)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                        }
                        else
                        {
                            if (DataInicio.Day == 29 && DataFim.Day == 29 && DataInicioDay == 29)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicio.Day == 29 && DataFim.Day >= 28 && DataInicioDay == 29 && DataFimDay >= 28)
                            {
                                //DataInicio igual a 29 e DataFim menor igual que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicio.Day == 29 && DataFim.Day < 28 && DataInicioDay == 29 && DataFimDay < 28)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicio.Day <= 28 && DataFim.Day <= 28 && DataInicioDay <= 28 && DataFimDay <= 28)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicio.Day >= 30 && DataFim.Day >= 30 && DataInicioDay >= 30 && DataFimDay >= 30)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                        }

                        DataInicio = DataInicio.AddYears(dados.Repetir);//Adicona ano 
                        DataFim = DataFim.AddYears(dados.Repetir);//Adicona ano
                    }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }
            else if (dados.Termina == 2)//Termina na Data
            {
                if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.Update(new Visita()
                    {
                        IdVisita = dados.IdVisita,
                        Descricao = dados.Descricao,
                        DataAgendamento = DateTime.Now,
                        DataInicio = DataInicio,
                        DataFim = DataFim,
                        DataCadastro = DateTime.Now,
                        Observacao = dados.Observacao,
                        IdStatusVisita = IdStatusVisita,
                        Visivel = Visivel,
                        Tipo = dados.Tipo,
                         GuidId = GuidId.ToString(),
                        Cor = dados.Cor,
                        IdUsuario = IdUsuario,
                        Repete = dados.Repete,
                        Frequencia = dados.Frequencia,
                        Repetir = dados.Repetir,
                        Termina = dados.Termina,
                        DataTerminaEm = dados.DataTerminaEm
                    });
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 1)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
                else
                {
                    while (dados.DataTerminaEm >= new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day))
                    {

                        if (DateTime.IsLeapYear(DataInicio.Year))
                        {
                            if (DataInicioDay == 29)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicioDay == 29 && DataFimDay >= 28)
                            {
                                //DataInicio igual a 29 e DataFim menor igual que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicioDay == 29 && DataFimDay < 28)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicioDay <= 28 && DataFimDay <= 28)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicioDay >= 30 && DataFimDay >= 30)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                        }
                        else
                        {
                            if (DataInicio.Day == 29 && DataFim.Day == 29 && DataInicioDay == 29)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicio.Day == 29 && DataFim.Day >= 28 && DataInicioDay == 29 && DataFimDay >= 28)
                            {
                                //DataInicio igual a 29 e DataFim menor igual que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicio.Day == 29 && DataFim.Day < 28 && DataInicioDay == 29 && DataFimDay < 28)
                            {
                                //DataInicio igual a 29 e DataFim menor que 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }

                            }
                            else if (DataInicio.Day <= 28 && DataFim.Day <= 28 && DataInicioDay <= 28 && DataFimDay <= 28)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                       DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                            else if (DataInicio.Day >= 30 && DataFim.Day >= 30 && DataInicioDay >= 30 && DataFimDay >= 30)
                            {
                                //DataInicio e DataFim menor igual a 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                        Frequencia = dados.Frequencia,
                                        Repetir = dados.Repetir,
                                        Termina = dados.Termina,
                                        DataTerminaEm = dados.DataTerminaEm
                                    }, dados.GuidId);
                                }
                            }
                        }

                        DataInicio = DataInicio.AddYears(dados.Repetir);//Adicona ano 
                        DataFim = DataFim.AddYears(dados.Repetir);//Adicona ano
                    }
                    //Atualiza todos com o mesmo GuidId anterior
                    if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                    }
                }
            }

        }

        private void Nunca(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if(dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            if (dados.IdVisita == 0)
            {
                Business.VisitaModel.Add(new Visita()
                {
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = dados.DataInicio,
                    DataFim = dados.DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                    GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
            }
            else if(dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = dados.DataInicio,
                    DataFim = dados.DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                     GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
            }
        }

        private void TodosOsDias(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;

            var DataInicioYear = DataInicio.Year + 2;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 8;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            dados.DataInicio = dados.DataInicio.AddYears(2);

            if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = dados.Visivel,
                    Tipo = dados.Tipo,
                    GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
            else
            {
                do
                {
                    if (dados.IdVisita == 0)
                    {
                        Business.VisitaModel.Add(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        });
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                    {
                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                        {
                            IdVisita = dados.IdVisita,
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = dados.Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.IdVisita, dados.GuidId);
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                        {
                            IdVisita = dados.IdVisita,
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = dados.Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.GuidId);
                    }
                    DataInicio = DataInicio.AddDays(1);
                    DataFim = DataFim.AddDays(1);

                } while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond));
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
        }

        private void CadaSemana(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            int j = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;

            dados.DataInicio = dados.DataInicio.AddYears(14);

            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                    GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
            else
            {
                do
                {
                    if (dados.IdVisita == 0)
                    {
                        Business.VisitaModel.Add(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        });
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                    {
                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.IdVisita, dados.GuidId);
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.GuidId);
                    }
                    DataInicio = myCal.AddWeeks(DataInicio, 1);
                    DataFim = myCal.AddWeeks(DataFim, 1);
                    j++;
                } while (j < 200);
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
           
        }

        private void CadaDuasSemana(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            int j = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;

            var DataInicioYear = DataInicio.Year;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 8;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            dados.DataInicio = dados.DataInicio.AddYears(28);

            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                    GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
            else
            {

                do
                {
                    if (dados.IdVisita == 0)
                    {
                        Business.VisitaModel.Add(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                            GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        });
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                    {
                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                             GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.IdVisita, dados.GuidId);
                    }
                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                    {
                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                        {
                            Descricao = dados.Descricao,
                            DataAgendamento = DateTime.Now,
                            DataInicio = DataInicio,
                            DataFim = DataFim,
                            DataCadastro = DateTime.Now,
                            Observacao = dados.Observacao,
                            IdStatusVisita = IdStatusVisita,
                            Visivel = Visivel,
                            Tipo = dados.Tipo,
                             GuidId = GuidId.ToString(),
                            Cor = dados.Cor,
                            IdUsuario = IdUsuario,
                            Repete = dados.Repete,
                        }, dados.GuidId);
                    }
                    DataInicio = myCal.AddWeeks(DataInicio, 2);
                    DataFim = myCal.AddWeeks(DataFim, 2);
                    j++;
                } while (j < 200);
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }

        }

        private void CadaMes(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            int j = 0;
            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 && dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;

            dados.DataInicio = dados.DataInicio.AddYears(10);

            var DataInicioYear = DataInicio.Year + 10;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 60;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            var DayOfWeek = DataInicio.DayOfWeek.ToString();

            //Usa o calend�rio padr�o do InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                    GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
            else
            {
                while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond))
                {
                    if (dados.SelectMensalmente == 1)//Mensalmente no dia X
                    {
                        if (dados.MesDia == DataInicio.Day)
                        {
                            if (DataInicio.Day == 31 && dados.MesDia == DataInicio.Day)//Dia 31
                            {
                                if (DataInicio.Day == 31 && DataFim.Day == 31)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 31 && DataFim.Day == 30)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 31 && DataFim.Day == 29)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 31 && DataFim.Day == 28)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 31 && DataFim.Day < 28)
                                {
                                    //DataInicio igual a 31 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                            }
                            else if (DataInicio.Day == 30 && dados.MesDia == DataInicio.Day)//Dia 30
                            {
                                if (DataInicio.Day == 30 && DataFim.Day == 30)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 30 && DataFim.Day == 29)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 30 && DataFim.Day == 28)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 30 && DataFim.Day < 28)
                                {
                                    //DataInicio igual a 30 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                            }
                            else if (DataInicio.Day == 29 && dados.MesDia == DataInicio.Day)//Dia 29
                            {
                                var a = DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month);
                                if (DataInicio.Day == 29 && DataFim.Day == 28)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 29 && DataFim.Day == 29)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 29 && DataFim.Day == 30)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 29 && DataFim.Day == 31)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                else if (DataInicio.Day == 29 && DataFim.Day < 28)
                                {
                                    //DataInicio igual a 29 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                }
                                DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                            }
                            else if (DataInicio.Day <= 28 && DataInicio.Day <= 28 && dados.MesDia == DataInicio.Day)//Dia 29
                            {
                                //DataInicio menor igual 28 e DataFim menor igual 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    }, dados.GuidId);
                                }
                                DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                                DataInicio = DataInicio.AddDays(-16);//Adicona um Dia 
                                DataFim = DataFim.AddDays(-16);//Adicona um Dia 
                            }
                        }
                        DataInicio = DataInicio.AddDays(1);//Adicona um Dia 
                        DataFim = DataFim.AddDays(1);//Adicona um Dia 
                        if (!DateTime.IsLeapYear(DataInicio.Year) && DataInicio.Month == 2 && DataInicio.Day == 28 && dados.MesDia == 29)//anos que n�o s�o bissexto, no m�s de fevereiro e no dia 28
                        {
                            DataInicio = DataInicio.AddMonths(dados.Repetir);//Adicona um M�s 
                            DataFim = DataFim.AddMonths(dados.Repetir);//Adicona um M�s 
                        }
                    }


                    if (dados.SelectMensalmente == 2)//Se for selecionado a op��o 2, buscando dia da semana e coloca��o do dia.
                    {
                        DayOfWeek = DataInicio.DayOfWeek.ToString();
                        if (dados.MesDataColocacao == Util.ObterClassificacao(DataInicio) && dados.MesDiaDaSemana == DayOfWeek)
                        {

                            if (DataInicio.Day == 31)//Dia 31
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 31 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 31 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 31 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 31, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                            }
                            else if (DataInicio.Day == 30)//Dia 30
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 30 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 30 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 30 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 30, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }

                            }
                            else if (DataInicio.Day == 29)//Dia 29
                            {
                                if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 28)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 28, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 29)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 29
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 30)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 30
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 30, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) == 31)
                                {
                                    //DataInicio igual a 29 e DataFim igual a 31
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 31, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                                else if (DateTime.DaysInMonth(DataInicio.Year, DataInicio.Month) == 29 && DateTime.DaysInMonth(DataFim.Year, DataFim.Month) < 28)
                                {
                                    //DataInicio igual a 29 e DataFim menor que 28
                                    if (dados.IdVisita == 0)
                                    {
                                        Business.VisitaModel.Add(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                            GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        });
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                    {
                                        Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.IdVisita, dados.GuidId);
                                    }
                                    else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                    {
                                        Business.VisitaModel.UpdateTodosEventos(new Visita()
                                        {
                                            Descricao = dados.Descricao,
                                            DataAgendamento = DateTime.Now,
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                            DataCadastro = DateTime.Now,
                                            Observacao = dados.Observacao,
                                            IdStatusVisita = IdStatusVisita,
                                            Visivel = Visivel,
                                            Tipo = dados.Tipo,
                                             GuidId = GuidId.ToString(),
                                            Cor = dados.Cor,
                                            IdUsuario = IdUsuario,
                                            Repete = dados.Repete,
                                        }, dados.GuidId);
                                    }
                                    DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                    DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                    DataInicio = myCal.AddDays(DataInicio, -16);
                                    DataFim = myCal.AddDays(DataFim, -16);
                                }
                            }
                            else if (DataInicio.Day <= 28 && DataFim.Day <= 28)//Dia 28
                            {
                                //DataInicio menor igual 28 e DataFim menor igual 28
                                if (dados.IdVisita == 0)
                                {
                                    Business.VisitaModel.Add(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                        GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    });
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                                {
                                    Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    }, dados.IdVisita, dados.GuidId);
                                }
                                else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                                {
                                    Business.VisitaModel.UpdateTodosEventos(new Visita()
                                    {
                                        Descricao = dados.Descricao,
                                        DataAgendamento = DateTime.Now,
                                        DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                        DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                        DataCadastro = DateTime.Now,
                                        Observacao = dados.Observacao,
                                        IdStatusVisita = IdStatusVisita,
                                        Visivel = Visivel,
                                        Tipo = dados.Tipo,
                                         GuidId = GuidId.ToString(),
                                        Cor = dados.Cor,
                                        IdUsuario = IdUsuario,
                                        Repete = dados.Repete,
                                    }, dados.GuidId);
                                }
                                DataInicio = myCal.AddMonths(DataInicio, dados.Repetir);
                                DataFim = myCal.AddMonths(DataFim, dados.Repetir);

                                DataInicio = myCal.AddDays(DataInicio, -16);
                                DataFim = myCal.AddDays(DataFim, -16);
                            }
                        }
                        DataInicio = myCal.AddDays(DataInicio, 1);//Adicona uma Semana 
                        DataFim = myCal.AddDays(DataFim, 1); ;//Adicona uma Semana 
                    }

                }
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
        }

        private void CadaAno(EnviarCalendarioSazonalViewModel dados)
        {
            byte? Visivel = 0;
            Guid GuidId;
            Visivel = VisivelPara();
            GuidId = ObterGuidId();
            long IdUsuario = GetIdUsuario();
            byte? IdStatusVisita = 0;
            CalendarioSazonal CalendarioSazonal = null;
            long? IdCalendarioSazonal;
            int antes = 5;
            int depois = 3;

            if (dados.Tipo == (byte)3)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.Agendada;
            }
            else if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
            {
                IdStatusVisita = (byte)EnumeradorModel.StatusVisita.FeriadoDataSazonal;
            }

            DateTime DataInicio = dados.DataInicio;
            DateTime DataFim = dados.DataFim;

            var DataInicioYear = DataInicio.Year + 10;
            var DataInicioMonth = DataInicio.Month;
            var DataInicioDay = DataInicio.Day;
            var DataInicioHour = DataInicio.Hour;
            var DataInicioMinute = DataInicio.Minute;
            var DataInicioSecond = DataInicio.Second;

            var DataFimYear = DataFim.Year + 60;
            var DataFimMonth = DataFim.Month;
            var DataFimDay = DataFim.Day;
            var DataFimHour = DataFim.Hour;
            var DataFimMinute = DataFim.Minute;
            var DataFimSecond = DataFim.Second;

            var DayOfWeek = DataInicio.DayOfWeek.ToString();

            //Usa o calend�rio padr�o do InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 1)
            {
                Business.VisitaModel.Update(new Visita()
                {
                    IdVisita = dados.IdVisita,
                    Descricao = dados.Descricao,
                    DataAgendamento = DateTime.Now,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    DataCadastro = DateTime.Now,
                    Observacao = dados.Observacao,
                    IdStatusVisita = IdStatusVisita,
                    Visivel = Visivel,
                    Tipo = dados.Tipo,
                     GuidId = GuidId.ToString(),
                    Cor = dados.Cor,
                    IdUsuario = IdUsuario,
                    Repete = dados.Repete,
                });
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 1)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
            else
            {
                IdCalendarioSazonal = null;
                while (DataInicio < new DateTime(DataInicioYear, DataInicioMonth, DataInicioDay, DataInicioHour, DataInicioMinute, DataInicioSecond))
                {
                    if (DateTime.IsLeapYear(DataInicio.Year))
                    {
                        if (DataInicioDay == 29)
                        {
                            //DataInicio igual a 29 e DataFim menor que 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }

                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicioDay == 29 && DataFimDay >= 28)
                        {
                            //DataInicio igual a 29 e DataFim menor igual que 28
                            if (dados.IdVisita == 0)
                            {
                                if(dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }

                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }

                        }
                        else if (DataInicioDay == 29 && DataFimDay < 28)
                        {
                            //DataInicio igual a 29 e DataFim menor que 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);
                                
                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicioDay <= 28 && DataFimDay <= 28)
                        {
                            //DataInicio e DataFim menor igual a 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }

                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicioDay >= 30 && DataFimDay >= 30)
                        {
                            //DataInicio e DataFim menor igual a 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                    }
                    else
                    {
                        if (DataInicio.Day == 29 && DataFim.Day == 29 && DataInicioDay == 29)
                        {
                            //DataInicio igual a 29 e DataFim menor que 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }

                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, 29, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicio.Day == 29 && DataFim.Day >= 28 && DataInicioDay == 29 && DataFimDay >= 28)
                        {
                            //DataInicio igual a 29 e DataFim menor igual que 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }

                        }
                        else if (DataInicio.Day == 29 && DataFim.Day < 28 && DataInicioDay == 29 && DataFimDay < 28)
                        {
                            //DataInicio igual a 29 e DataFim menor que 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicio.Day <= 28 && DataFim.Day <= 28 && DataInicioDay <= 28 && DataFimDay <= 28)
                        {
                            //DataInicio e DataFim menor igual a 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                        else if (DataInicio.Day >= 30 && DataFim.Day >= 30 && DataInicioDay >= 30 && DataFimDay >= 30)
                        {
                            //DataInicio e DataFim menor igual a 28
                            if (dados.IdVisita == 0)
                            {
                                if (dados.IdCalendarioSazonal == 0)
                                {
                                    if (dados.Tipo == (byte)1 || dados.Tipo == (byte)2)
                                    {
                                        Business.CalendarioSazonalModel.Add(new CalendarioSazonal()
                                        {
                                            Descricao = dados.Descricao,
                                            Cor = dados.Cor,
                                            Tipo = dados.Tipo,
                                            DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, 29),
                                            DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, 29, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                            DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFimDay, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                            DataCadastro = DateTime.Now,
                                            ExisteCampanha = dados.ExisteCampanha,
                                            Ativo = dados.Ativo,
                                            GuidId = GuidId.ToString(),
                                            Repete = dados.Repete,
                                            Frequencia = dados.Frequencia,
                                        });
                                        CalendarioSazonal = Business.CalendarioSazonalModel.GetLastId();
                                        IdCalendarioSazonal = CalendarioSazonal.IdCalendarioSazonal;
                                    }
                                }
                                Business.VisitaModel.Add(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                    GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                    IdCalendarioSazonal = IdCalendarioSazonal,
                                });
                                IdCalendarioSazonal = null;
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 2)
                            {
                                Business.VisitaModel.UpdateEventosSeguintes(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.IdVisita, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateEventosSeguintes(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.IdCalendarioSazonal, dados.GuidId);
                            }
                            else if (dados.IdVisita > 0 && dados.OpExcluirAlterar == 3)
                            {
                                Business.VisitaModel.UpdateTodosEventos(new Visita()
                                {
                                    Descricao = dados.Descricao,
                                    DataAgendamento = DateTime.Now,
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second),
                                    DataCadastro = DateTime.Now,
                                    Observacao = dados.Observacao,
                                    IdStatusVisita = IdStatusVisita,
                                    Visivel = Visivel,
                                    Tipo = dados.Tipo,
                                     GuidId = GuidId.ToString(),
                                    Cor = dados.Cor,
                                    IdUsuario = IdUsuario,
                                    Repete = dados.Repete,
                                }, dados.GuidId);

                                Business.CalendarioSazonalModel.UpdateTodosEventos(new CalendarioSazonal()
                                {
                                    Descricao = dados.Descricao,
                                    Cor = dados.Cor,
                                    Tipo = dados.Tipo,
                                    DataSazonal = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day),
                                    DataInicio = new DateTime(DataInicio.Year, DataInicio.Month, DataInicio.Day, DataInicio.Hour, DataInicio.Minute, DataInicio.Second).AddDays(-antes),
                                    DataFim = new DateTime(DataFim.Year, DataFim.Month, DataFim.Day, DataFim.Hour, DataFim.Minute, DataFim.Second).AddDays(depois),
                                    DataCadastro = DateTime.Now,
                                    ExisteCampanha = dados.ExisteCampanha,
                                    Ativo = dados.Ativo,
                                     GuidId = GuidId.ToString(),
                                    Repete = dados.Repete,
                                    Frequencia = dados.Frequencia,
                                }, dados.GuidId);
                            }
                        }
                    }

                    DataInicio = DataInicio.AddYears(dados.Repetir);//Adicona ano 
                    DataFim = DataFim.AddYears(dados.Repetir);//Adicona ano
                }
                //Atualiza todos com o mesmo GuidId anterior
                if (dados.OpExcluirAlterar == 2 || dados.OpExcluirAlterar == 3)
                {
                    Business.VisitaModel.AtualizaGuiId(GuidId.ToString(), dados.GuidId);
                }
            }
        }

        private bool DataExiste(DateTime Data)
        {
            bool status = false;
            try
            {
                var dt = new DateTime(Data.Year, Data.Month, Data.Day, Data.Hour, Data.Minute, Data.Second);
                status = true;
            }
            catch
            {
                return status;
            }
            return status;
        }

        private byte VisivelPara()
        {
            long IdUsuario = GetIdUsuario();

            UsuarioPerfil EntityUsuarioPerfil = PerfilModel.GetIdentificacaoPerfil(IdUsuario);
            byte Visivel = 0;
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

            return Visivel;
        }

        private Guid ObterGuidId()
        {
            Guid GuidId;
            do
            {
                GuidId = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidId));
            return GuidId;
        }

        private void GeraNotificacao(EnviarCalendarioSazonalViewModel dados)
        {
            long IdUsuario = GetIdUsuario();
            UsuarioHierarquia EntityUsuarioHierarquia = UsuarioHierarquiaModel.GetSlave(IdUsuario);
            if (EntityUsuarioHierarquia != null)
            {
                Visita EntityVisita = Business.VisitaModel.GetLastId();
                Notificacao EntityNotificacao = NotificacaoModel.Add(new Notificacao()
                {
                    IdUsuarioCadastro = IdUsuario,
                    IdUsuarioVisualizar = EntityUsuarioHierarquia.IdUsuarioMaster,
                    Titulo = null,
                    Descricao = dados.Descricao,
                    Url = "/Visita?Id=" + HttpUtility.UrlEncode(Criptography.Encrypt(EntityVisita.IdVisita.ToString())),
                    Visualizado = false,
                    DataCadastro = DateTime.Now,
                    Ativo = true
                });
            }
        }

    }
   #endregion
}