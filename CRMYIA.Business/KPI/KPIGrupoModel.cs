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
    public class KPIGrupoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIGrupo Get(long IdKPIGrupo)
        {
            KPIGrupo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupo
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPIGrupo == IdKPIGrupo)
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

        public static List<KPIGrupo> GetList()
        {
            List<KPIGrupo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIGrupo
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Include(y => y.KPIGrupoUsuario)
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

        public static void Add(KPIGrupo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIGrupo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupo.Update(Entity);
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
