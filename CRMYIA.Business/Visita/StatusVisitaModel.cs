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
    public class StatusVisitaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static StatusVisita Get(long IdStatusVisita)
        {
            StatusVisita Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.StatusVisita
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdStatusVisita == IdStatusVisita)
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

        public static List<StatusVisita> GetList()
        {
            List<StatusVisita> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusVisita
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdStatusVisita).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<StatusVisita> GetListIdDescricao()
        {
            List<StatusVisita> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusVisita
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new StatusVisita()
                        {
                            IdStatusVisita = y.IdStatusVisita,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdStatusVisita).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(StatusVisita Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusVisita.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(StatusVisita Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusVisita.Update(Entity);
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
