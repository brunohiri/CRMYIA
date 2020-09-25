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
    public class HistoricoPropostaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static HistoricoProposta Get(long IdHistoricoProposta)
        {
            HistoricoProposta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.HistoricoProposta
                        .AsNoTracking()
                        .Where(x =>x.IdHistoricoProposta == IdHistoricoProposta)
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

        public static List<HistoricoProposta> GetList()
        {
            List<HistoricoProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.HistoricoProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdHistoricoProposta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<HistoricoProposta> GetList(long IdProposta)
        {
            List<HistoricoProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.HistoricoProposta
                        .Include(y => y.IdUsuarioNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProposta == IdProposta)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataCadastro).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(HistoricoProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoProposta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(HistoricoProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoProposta.Update(Entity);
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
