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
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class MetaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Meta Get(long IdMeta)
        {
            Meta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Meta
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdMeta == IdMeta)
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

        public static List<Meta> GetList()
        {
            List<Meta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Meta
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.DataMaxima).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Meta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Meta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Meta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Meta.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Rollback(Meta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Meta.Remove(context.Meta.Find(Entity.IdMeta));
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
