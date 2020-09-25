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
    public class StatusPropostaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static StatusProposta Get(long IdStatusProposta)
        {
            StatusProposta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.StatusProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdStatusProposta == IdStatusProposta)
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

        public static List<StatusProposta> GetList()
        {
            List<StatusProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdStatusProposta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<StatusProposta> GetListIdDescricao()
        {
            List<StatusProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new StatusProposta()
                        {
                            IdStatusProposta = y.IdStatusProposta,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdStatusProposta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(StatusProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusProposta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(StatusProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusProposta.Update(Entity);
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
