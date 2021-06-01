using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class CalendarioModel
    {
        public static List<Calendario> GetList()
        {
            List<Calendario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Calendario
                        .Where(x => x.Ativo)
                        .OrderBy(o => o.Descricao)
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
        public static long Add(Calendario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Calendario.Add(Entity);
                    context.SaveChanges();
                    return Entity.IdCalendario;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
