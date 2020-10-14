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
                        .AsNoTracking()
                        .Where(x => x.IdVisita == IdVisita)
                        .AsNoTracking()
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
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Visita
                        .Include(y => y.IdStatusVisitaNavigation)
                        .Include(y => y.IdPropostaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdPropostaNavigation.IdUsuario == IdUsuario && (DataInicial.HasValue ? (x.DataAgendamento >= DataInicial.Value) : true) && (DataFinal.HasValue ? (x.DataAgendamento <= DataFinal.Value) : true))
                        .AsNoTracking()
                        .OrderBy(o => o.DataAgendamento).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<VisitaViewModel> GetListByDataAgendamentoReturnsViewModel(long IdUsuario, DateTime? DataInicial, DateTime? DataFinal)
        {
            List<VisitaViewModel> ListEntityViewModel = null;
            List<Visita> ListEntity = null;
            try
            {
                ListEntity = GetListByDataAgendamento(IdUsuario, DataInicial, DataFinal);
                if (ListEntity != null && ListEntity.Count() > 0)
                    ListEntityViewModel = ListEntity.Select(x => new VisitaViewModel()
                    {
                        //id = x.IdVisita,
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
        #endregion
    }
}
