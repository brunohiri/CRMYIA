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
    public class KPICargoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPICargo Get(int IdKPICargo)
        {
            KPICargo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPICargo
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPICargo == IdKPICargo)
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

        public static List<KPICargo> GetList()
        {
            List<KPICargo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPICargo
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.Cargo).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<KPICargo> GetListIdCargo()
        {
            List<KPICargo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPICargo
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new KPICargo()
                        {
                            IdKPICargo = y.IdKPICargo,
                            Cargo = y.Cargo
                        }).OrderBy(o => o.Cargo).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(KPICargo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPICargo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPICargo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPICargo.Update(Entity);
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
