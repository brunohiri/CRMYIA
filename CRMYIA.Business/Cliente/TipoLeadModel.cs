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
    public class TipoLeadModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static TipoLead Get(long IdTipoLead)
        {
            TipoLead Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.TipoLead
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdTipoLead == IdTipoLead)
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

        public static List<TipoLead> GetList()
        {
            List<TipoLead> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.TipoLead
                        .Where(x => x.Ativo)
                        .Include(KPICargo => KPICargo.IdKPICargoNavigation)
                        .Include(KPIServico => KPIServico.IdKPIServicoNavigation)
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

        public static List<TipoLead> GetListIdDescricao()
        {
            List<TipoLead> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.TipoLead
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new TipoLead()
                        {
                            IdTipoLead = y.IdTipoLead,
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

        public static void Add(TipoLead Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.TipoLead.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(TipoLead Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.TipoLead.Update(Entity);
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
