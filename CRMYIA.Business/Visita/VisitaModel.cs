using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class VisitaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Visita Get(long IdVisita)
        {
            Visita Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Where(x => x.IdVisita == IdVisita)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static bool ExisteMaisQueUm(string GuidId)
        {
            List<Visita> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Where(x => x.GuidId == GuidId)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity.Count > 1;
        }

        public static Visita GetVisitaId(long IdVisita)
        {
            Visita Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Where(x => x.IdVisita == IdVisita)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Visita GetByIdProposta(long IdProposta)
        {
            Visita Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Include(y => y.IdPropostaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdProposta == IdProposta)
                        .AsNoTracking()
                        .OrderByDescending(z=>z.DataCadastro)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Visita> GetList(long IdUsuario)
        {
            List<Visita> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Include(y => y.IdPropostaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdPropostaNavigation.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .OrderBy(o => o.IdVisita).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Visita> GetListByDataAgendamento(long IdUsuario, DateTime? DataInicial, DateTime? DataFinal)
        {
            List<Visita> ListEntity = null;
            try
            {
                if(IdUsuario != 0)
                {
                    using (YiaContext context = new YiaContext())
                    {
                        ListEntity = context.Visita
                            .Include(y => y.IdStatusVisitaNavigation)
                            .Include(y => y.IdCalendarioSazonalNavigation)
                            .Include(y => y.IdPropostaNavigation)
                            .AsNoTracking()
                            .Where(x => x.Visivel == (byte)Business.Util.EnumeradorModel.Visualizacao.Todos || x.Tipo == 3 || (x.IdUsuario == IdUsuario && (DataInicial.HasValue ? (x.DataAgendamento >= DataInicial.Value) : true) && (DataFinal.HasValue ? (x.DataAgendamento <= DataFinal.Value) : true)))
                            .AsNoTracking()
                            .OrderBy(o => o.DataAgendamento).ToList();
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Visita> GetListByDataAgendamento(long IdPerfil, DateTime? DataInicial, DateTime? DataFinal, string Nome = "", int IdPerfilUsuario = 0)
        {
            List<Visita> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    switch (IdPerfil)
                    {
                        case 0:
                            {

                                switch (IdPerfilUsuario)
                                {
                                    
                                    case 1:
                                        {
                                            return ListEntity = context.Visita
                                           .Include(y => y.IdStatusVisitaNavigation)
                                           .Include(y => y.IdCalendarioSazonalNavigation)
                                           .Include(y => y.IdPropostaNavigation)
                                           .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                           .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                           .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                           .Where(pp =>  (pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario)) && 
                                            (pp.o.u.Nome.Contains(Nome))).OrderBy(p => p.o.u.Nome)
                                           .Select(pp => pp.o.v).ToList();
                                        }
                                    case 2:
                                        {
                                            return ListEntity = context.Visita
                                            .Include(y => y.IdStatusVisitaNavigation)
                                            .Include(y => y.IdCalendarioSazonalNavigation)
                                            .Include(y => y.IdPropostaNavigation)
                                            .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                            .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                            .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                            .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                                            ((Int32?)(pp.up.IdPerfil) != (Int32?)1) && 
                                            (pp.o.u.Nome.Contains(Nome)))).OrderBy(p => p.o.u.Nome) 
                                            .Select(pp => pp.o.v).ToList();
                                        }
                                    case 3:
                                        {
                                            return ListEntity = context.Visita
                                            .Include(y => y.IdStatusVisitaNavigation)
                                            .Include(y => y.IdCalendarioSazonalNavigation)
                                            .Include(y => y.IdPropostaNavigation)
                                            .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                            .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                            .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                            .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                                            ((Int32?)(pp.up.IdPerfil) != (Int32?)1) && ((Int32?)(pp.up.IdPerfil) != (Int32?)2) && 
                                            (pp.o.u.Nome.Contains(Nome)))).OrderBy(p => p.o.u.Nome)
                                            .Select(pp => pp.o.v).ToList();
                                        }
                                    case 4:
                                        {
                                            return ListEntity = context.Visita
                                            .Include(y => y.IdStatusVisitaNavigation)
                                            .Include(y => y.IdCalendarioSazonalNavigation)
                                            .Include(y => y.IdPropostaNavigation)
                                            .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                            .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                            .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                            .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                                            ((Int32?)(pp.up.IdPerfil) != (Int32?)1) && ((Int32?)(pp.up.IdPerfil) != (Int32?)2) && ((Int32?)(pp.up.IdPerfil) != (Int32?)3) && 
                                            (pp.o.u.Nome.Contains(Nome)))).OrderBy(p => p.o.u.Nome)
                                            .Select(pp => pp.o.v).ToList();
                                        }
                                    default:
                                        {
                                            return ListEntity;
                                        }
                                }

                               return ListEntity;
                                   
                            }
                        case 2:
                            {
                                return ListEntity = context.Visita
                                        .Include(y => y.IdStatusVisitaNavigation)
                                        .Include(y => y.IdCalendarioSazonalNavigation)
                                        .Include(y => y.IdPropostaNavigation)
                                        .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                        .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                        .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                        .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                                        ((Int32?)(pp.up.IdPerfil) != (Int32?)1))&& (pp.o.u.Nome.Contains(Nome))).OrderBy(p => p.o.u.Nome)
                                        .Select(pp => pp.o.v).ToList();
                            }
                        case 3:
                            {
                                return ListEntity = context.Visita
                                .Include(y => y.IdStatusVisitaNavigation)
                                .Include(y => y.IdCalendarioSazonalNavigation)
                                .Include(y => y.IdPropostaNavigation)
                                .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                                .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                                .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                                .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                                ((Int32?)(pp.up.IdPerfil) != (Int32?)1)) &&
                                ((Int32?)(pp.up.IdPerfil) != (Int32?)2) && (pp.o.u.Nome.Contains(Nome))).OrderBy(p => p.o.u.Nome)
                                .Select(pp => pp.o.v).ToList();
                            }
                        case 4:
                            {
                                return ListEntity = context.Visita
                               .Include(y => y.IdStatusVisitaNavigation)
                               .Include(y => y.IdCalendarioSazonalNavigation)
                               .Include(y => y.IdPropostaNavigation)
                               .Join(context.Usuario, v => v.IdUsuario, u => (Int64?)(u.IdUsuario), (v, u) => new { v = v, u = u })
                               .Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) => new { o = o, up = up })
                               .Where(c => !(c.o.v.DataAgendamento > DataFinal || c.o.v.DataAgendamento < DataInicial))
                               .Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario))))) &&
                               ((Int32?)(pp.up.IdPerfil) != (Int32?)1)) &&
                               ((Int32?)(pp.up.IdPerfil) != (Int32?)2) && ((Int32?)(pp.up.IdPerfil) != (Int32?)3) && (pp.o.u.Nome.Contains(Nome))).OrderBy(p => p.o.u.Nome)
                               .Select(pp => pp.o.v).ToList();
                            }
                        default:
                            {
                                return ListEntity;
                            }
                    }
                    // ListEntity = context.Visita
                    //.Include(y => y.IdStatusVisitaNavigation)
                    //.Include(y => y.IdPropostaNavigation)
                    //.Join(context.Usuario,v => v.IdUsuario,u => (Int64?)(u.IdUsuario), (v, u) =>new{v = v,u = u})
                    //.Join(context.UsuarioPerfil, o => (Int64?)(o.u.IdUsuario), up => up.IdUsuario, (o, up) =>new {o = o,up = up})
                    //.Where(pp => (((((pp.o.v.IdUsuario == (Int64?)(pp.o.u.IdUsuario)) &&
                    //((Int32?)(pp.up.IdPerfil) != (Int32?)1)) &&
                    //((Int32?)(pp.up.IdPerfil) != (Int32?)2)) &&
                    //((Int32?)(pp.up.IdPerfil) != (Int32?)0)) &&
                    //((Int32?)(pp.up.IdPerfil) != (Int32?)4)))
                    //.Select(pp => pp.o.v).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VisitaViewModel> GetListByDataAgendamentoReturnsViewModel(long IdUsuario, DateTime? DataInicial, DateTime? DataFinal)
        {
            //List<VisitaViewModel> ListEntityViewModel = null;
            List<VisitaViewModel> ListEntityViewModel = new List<VisitaViewModel>();
            List<Visita> ListEntity = null;
            try
            {
                ListEntity = GetListByDataAgendamento(IdUsuario, DataInicial, DataFinal);
                if (ListEntity != null && ListEntity.Count() > 0)
                {
                    UsuarioPerfil EntityUsuarioPerfil = Business.PerfilModel.GetIdentificacaoPerfil(IdUsuario);

                    if (EntityUsuarioPerfil.IdPerfil != (byte)6)
                    {

                        foreach (var Item in ListEntity)
                        {
                            ListEntityViewModel.Add(new VisitaViewModel()
                            {
                                sourceId = Item.IdVisita,
                                backgroundColor = Item.IdStatusVisitaNavigation.CorHexa,
                                borderColor = Item.IdStatusVisitaNavigation.CorHexa,
                                start = Item.DataAgendamento,
                                end = Item.DataFim,
                                title = Item.Descricao,
                                allDay = false,
                                Tipo = Item.Tipo
                            }
                            );
                        }
                        
                    }
                    else if (EntityUsuarioPerfil.IdPerfil == (byte)6)
                    {
                        foreach (var Item in ListEntity)
                        {

                            if (Item.IdCalendarioSazonalNavigation != null)
                            {
                                ListEntityViewModel.Add(new VisitaViewModel()
                                {
                                    sourceId = Item.IdVisita,
                                    backgroundColor = Item.IdCalendarioSazonalNavigation.Cor,
                                    borderColor = Item.IdCalendarioSazonalNavigation.Cor,
                                    start = Item.IdCalendarioSazonalNavigation.DataSazonal,
                                    end = Item.IdCalendarioSazonalNavigation.DataFim,
                                    title = Item.Descricao,
                                    allDay = false,
                                    Tipo = Item.Tipo
                                }
                                );

                                if (Item.IdCalendarioSazonalNavigation.ExisteCampanha == true)
                                {
                                    ListEntityViewModel.Add(new VisitaViewModel()
                                    {
                                        sourceId = Item.IdVisita,
                                        backgroundColor = Item.IdCalendarioSazonalNavigation.Cor,
                                        borderColor = Item.IdCalendarioSazonalNavigation.Cor,
                                        start = Convert.ToDateTime(Item.IdCalendarioSazonalNavigation.DataInicio?.ToString("yyyy-MM-dd")),
                                        end = Convert.ToDateTime(Item.IdCalendarioSazonalNavigation.DataFim?.ToString("yyyy-MM-dd")),
                                        title = "Período " + Item.IdCalendarioSazonalNavigation.Descricao,
                                        allDay = false,
                                        Tipo = Item.Tipo
                                    }
                                    );
                                }
                                //else
                                //{
                                //    ListEntityViewModel.Add(new VisitaViewModel()
                                //    {
                                //        sourceId = Item.IdVisita,
                                //        backgroundColor = Item.IdCalendarioSazonalNavigation.Cor,
                                //        borderColor = Item.IdCalendarioSazonalNavigation.Cor,
                                //        start = Item.IdCalendarioSazonalNavigation.DataInicio,
                                //        //end = Item.IdCalendarioSazonalNavigation.DataFim,
                                //        title = "Período " + Item.IdCalendarioSazonalNavigation.Descricao,
                                //        allDay = false
                                //    }
                                //   );
                                //}
                            }
                            else if (Item.Tipo == 3)
                            {
                                ListEntityViewModel.Add(new VisitaViewModel()
                                {
                                    sourceId = Item.IdVisita,
                                    backgroundColor = Item.Cor,
                                    borderColor = Item.Cor,
                                    start = Item.DataInicio,
                                    end = Item.DataFim,
                                    title = Item.Descricao,
                                    allDay = false,
                                    Tipo = Item.Tipo
                                }
                                );
                            }

                            //ListEntityViewModel = ListEntity.Select(x => new VisitaViewModel()
                            //{
                            //    sourceId = x.IdVisita,
                            //    backgroundColor = x.IdStatusVisitaNavigation.CorHexa,
                            //    borderColor = x.IdStatusVisitaNavigation.CorHexa,
                            //    start = x.DataAgendamento.Value,
                            //    title = x.Descricao,
                            //    allDay = false
                            //}).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntityViewModel;
        }

        public static List<VisitaViewModel> GetListByDataAgendamentoPesquisaViewModel(DateTime? DataInicial, DateTime? DataFinal, long IdPerfil = 0, string Nome = "", int IdPerfilUsuario = 0)
        {
            List<VisitaViewModel> ListEntityViewModel = null;
            List<Visita> ListEntity = null;
            try
            {
                ListEntity = GetListByDataAgendamento(IdPerfil, DataInicial, DataFinal, Nome, IdPerfilUsuario);
                if (ListEntity != null && ListEntity.Count() > 0)
                    ListEntityViewModel = ListEntity.Select(x => new VisitaViewModel()
                    {
                        sourceId = x.IdVisita,
                        backgroundColor = x.IdStatusVisitaNavigation.CorHexa,
                        borderColor = x.IdStatusVisitaNavigation.CorHexa,
                        start = x.DataAgendamento.Value,
                        title = x.Descricao,
                        allDay = false
                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntityViewModel;
        }

        public static void AddList(List<Visita> ListEntity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    foreach (Visita Item in ListEntity) 
                    {
                        context.Visita.Add(Item); 
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool VerificaGuidId(Guid GuidId)
        {
            List<Visita> EntityVisita = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    EntityVisita = context.Visita
                        .Where(x => x.GuidId == GuidId.ToString())
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return EntityVisita.Count > 0 ? true: false;
        }

        public static void Add(Visita Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Visita.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Visita Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Visita.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateEventosSeguintes(Visita Entity, long IdVisita, string GuidId)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    var alterarVisita = context.Visita
                  .Where(x => x.IdVisita >= IdVisita && x.GuidId == GuidId).First();

                    alterarVisita = Entity;

                    context.Visita.Attach(alterarVisita);
                    context.Entry(alterarVisita).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateTodosEventos(Visita Entity, string GuidId)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    var alterarVisita = context.Visita
                  .Where(x => x.GuidId == GuidId).First();

                    alterarVisita = Entity;

                    context.Visita.Attach(alterarVisita);
                    context.Entry(alterarVisita).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Visita GetLastId()
        {
            Visita Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Visita
                        .ToList()
                        .LastOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        #endregion
    }
}
