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
    public class MotivoDeclinioModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static MotivoDeclinio Get(long IdMotivoDeclinio)
        {
            MotivoDeclinio Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.MotivoDeclinio
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdMotivoDeclinio == IdMotivoDeclinio)
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

        public static List<MotivoDeclinio> GetList()
        {
            List<MotivoDeclinio> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.MotivoDeclinio
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdMotivoDeclinio).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<MotivoDeclinio> GetListIdDescricao()
        {
            List<MotivoDeclinio> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.MotivoDeclinio
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new MotivoDeclinio()
                        {
                            IdMotivoDeclinio = y.IdMotivoDeclinio,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdMotivoDeclinio).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(MotivoDeclinio Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.MotivoDeclinio.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(MotivoDeclinio Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.MotivoDeclinio.Update(Entity);
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
