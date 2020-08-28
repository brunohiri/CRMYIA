using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYIA.Business
{
    public class HistoricoAcessoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Task<HistoricoAcesso> Get(long IdUsuario)
        {
            Task<HistoricoAcesso> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.HistoricoAcesso
                        .Where(x => x.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Task<List<HistoricoAcesso>> GetList()
        {
            Task<List<HistoricoAcesso>> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.HistoricoAcesso
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(HistoricoAcesso Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoAcesso.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(HistoricoAcesso Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoAcesso.Update(Entity);
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
