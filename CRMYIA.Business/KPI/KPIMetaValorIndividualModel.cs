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
    public class KPIMetaValorIndividualModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIMetaValorIndividual Get(long IdMetaIndividual)
        {
            KPIMetaValorIndividual Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMetaValorIndividual
                        .AsNoTracking()
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

        public static List<KPIMetaValorIndividual> GetList()
        {
            List<KPIMetaValorIndividual> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaValorIndividual
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

        public static void Add(KPIMetaValorIndividual Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaValorIndividual.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIMetaValorIndividual Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaValorIndividual.Update(Entity);
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
