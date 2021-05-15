using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class KPIMetaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIMeta Get(long IdKPIUsuario)
        {
            KPIMeta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMeta
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPIGrupoNavigation.IdUsuario == IdKPIUsuario)
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

        public static List<KPIMeta> GetList()
        {
            List<KPIMeta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMeta
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

        public static void Add(KPIMeta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMeta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIMeta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMeta.Update(Entity);
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
