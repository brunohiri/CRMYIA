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
    public class KPIMetaValorModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIMetaValor Get(long IdKPIMetaValor)
        {
            KPIMetaValor Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMetaValor
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPIMetaValor == IdKPIMetaValor)
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

        public static List<KPIMetaValor> GetList()
        {
            List<KPIMetaValor> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaValor
                        .Where(x => x.Ativo)
                        .Include(Meta => Meta.IdMetaNavigation.IdKPIServicoNavigation)
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

        public static List<KPIMetaValor> GetListIdDescricao()
        {
            List<KPIMetaValor> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaValor
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new KPIMetaValor()
                        {
                            IdKPIMetaValor = y.IdKPIMetaValor,
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

        public static void Add(KPIMetaValor Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaValor.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIMetaValor Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaValor.Update(Entity);
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
