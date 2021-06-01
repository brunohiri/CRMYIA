using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class CalendarioSazonalModel
    {

        public static List<CalendarioSazonal> GetList()
        {
            List<CalendarioSazonal> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CalendarioSazonal
                       .Include(x => x.Visita)
                       //.Include(x => x.Campanha)
                       .AsNoTracking()
                       .Where(x => x.Ativo)
                       .AsNoTracking()
                       .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ListEntity;
        }

        public static CalendarioSazonal GetLastId()
        {
            CalendarioSazonal Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CalendarioSazonal
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

        public static void AddList(List<CalendarioSazonal> Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    foreach (CalendarioSazonal Item in Entity)
                    {
                        context.CalendarioSazonal.Add(Item);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Add(CalendarioSazonal Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CalendarioSazonal.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateEventosSeguintes(CalendarioSazonal Entity, long IdCalendarioSazonal, string GuidId)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    var alterarCalendarioSazonal = context.CalendarioSazonal
                  .Where(x => x.IdCalendarioSazonal >= IdCalendarioSazonal && x.GuidId == GuidId).First();

                    alterarCalendarioSazonal = Entity;

                    context.CalendarioSazonal.Attach(alterarCalendarioSazonal);
                    context.Entry(alterarCalendarioSazonal).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateTodosEventos(CalendarioSazonal Entity, string GuidId)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    var alterarCalendarioSazonal = context.CalendarioSazonal
                  .Where(x => x.GuidId == GuidId).First();

                    alterarCalendarioSazonal = Entity;

                    context.CalendarioSazonal.Attach(alterarCalendarioSazonal);
                    context.Entry(alterarCalendarioSazonal).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
