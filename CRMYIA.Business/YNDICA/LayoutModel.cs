using CRMYIA.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business.YNDICA
{
    public class LayoutModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Data.Entities.Layout Get(long IdLayout)
        {
            Data.Entities.Layout Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Layout
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdLayout == IdLayout)
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

        public static List<Data.Entities.Layout> GetList()
        {
            List<Data.Entities.Layout> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Layout
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }


        public static List<Data.Entities.Layout> GetListIdDescricao()
        {
            List<Data.Entities.Layout> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Layout
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Data.Entities.Layout()
                        {
                            IdLayout = y.IdLayout,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Data.Entities.Layout Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Layout.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Data.Entities.Layout Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Layout.Update(Entity);
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
