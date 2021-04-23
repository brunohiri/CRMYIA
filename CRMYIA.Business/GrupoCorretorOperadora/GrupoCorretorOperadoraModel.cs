using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class GrupoCorretorOperadoraModel
    {
        public static List<GrupoCorretorOperadora> Get(long IdOperadora)
        {
            List<GrupoCorretorOperadora> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.GrupoCorretorOperadora
                        .Include(x => x.IdOperadoraNavigation)
                        .Include(x => x.IdGrupoCorretorNavigation)
                        .Where(x => x.IdOperadora == IdOperadora)
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
        public static void Add(GrupoCorretorOperadora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.GrupoCorretorOperadora.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Delete(GrupoCorretorOperadora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {

                    context.GrupoCorretorOperadora.Remove(Entity);
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
