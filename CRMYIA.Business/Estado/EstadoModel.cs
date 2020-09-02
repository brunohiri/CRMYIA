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
    public class EstadoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Estado Get(long IdEstado)
        {
            Estado Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Estado
                        .Where(x => x.Ativo)
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

        public static List<Estado> GetList()
        {
            List<Estado> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Estado
                        .Where(x => x.Ativo)
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

        public static List<Estado> GetListIdSigla()
        {
            List<Estado> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Estado
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Estado()
                        {
                            IdEstado = y.IdEstado,
                            Sigla = y.Sigla
                        }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Estado Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Estado.AddAsync(Entity);
                    context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Estado Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Estado.Update(Entity);
                    context.SaveChangesAsync();
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
