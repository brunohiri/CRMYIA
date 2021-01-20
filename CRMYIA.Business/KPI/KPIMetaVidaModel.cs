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
    public class KPIMetaVidaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIMetaVida Get(long IdKPIMetaVida)
        {
            KPIMetaVida Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIMetaVida
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPIMetaVida == IdKPIMetaVida)
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

        public static List<KPIMetaVida> GetList()
        {
            List<KPIMetaVida> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaVida
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

        public static List<KPIMetaVida> GetListIdDescricao()
        {
            List<KPIMetaVida> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIMetaVida
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new KPIMetaVida()
                        {
                            IdKPIMetaVida = y.IdKPIMetaVida,
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

        public static void Add(KPIMetaVida Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaVida.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIMetaVida Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIMetaVida.Update(Entity);
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
