using CRMYIA.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business.YNDICA
{
    public class LayoutPJModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Data.Entities.LayoutPJ Get(long IdLayoutPJ)
        {
            Data.Entities.LayoutPJ Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.LayoutPJ
                        .AsNoTracking()
                        .Where(x => x.IdLayoutPJ == IdLayoutPJ)
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

        public static List<Data.Entities.LayoutPJ> GetByIdFila(long IdFila)
        {
            List<Data.Entities.LayoutPJ> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.LayoutPJ
                        .Include(y => y.IdFilaItemNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdFilaItemNavigation.IdFila == IdFila)
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Data.Entities.LayoutPJ> GetList()
        {
            List<Data.Entities.LayoutPJ> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.LayoutPJ
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }


        public static void Add(Data.Entities.LayoutPJ Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LayoutPJ.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Data.Entities.LayoutPJ Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LayoutPJ.Update(Entity);
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
