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
    public class KPIMetaIndividualModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIMetaIndividual Get(long IdKPIGrupoUsuario)
        {
            KPIMetaIndividual Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMetaIndividual.Include(x => x.KPIMetaValorIndividual).Include(y => y.KPIMetaVidaIndividual)
                        .Where(x => x.Ativo && x.IdKPIGrupoUsuario == IdKPIGrupoUsuario)
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
        public static KPIMetaIndividual GetByMeta(long IdMetaIndividual)
        {
            KPIMetaIndividual Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMetaIndividual.Include(x => x.KPIMetaValorIndividual).Include(y => y.KPIMetaVidaIndividual)
                        .Where(x => x.Ativo && x.IdMetaIndividual == IdMetaIndividual)
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

        public static List<KPIMetaIndividual> GetList()
        {
            List<KPIMetaIndividual> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaIndividual.Include(x => x.KPIMetaValorIndividual).Include(y => y.KPIMetaVidaIndividual)
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

        public static void Add(KPIMetaIndividual Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaIndividual.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIMetaIndividual Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaIndividual.Update(Entity);
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
