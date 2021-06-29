using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business.YNDICA
{
    public class StatusFilaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static StatusFila Get(long IdStatusFila)
        {
            StatusFila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.StatusFila
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdStatusFila == IdStatusFila)
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

        public static List<StatusFila> GetList()
        {
            List<StatusFila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusFila
                        .AsNoTracking()
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


        public static List<StatusFila> GetListIdDescricao()
        {
            List<StatusFila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.StatusFila
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new StatusFila()
                        {
                            IdStatusFila = y.IdStatusFila,
                            Descricao = y.Descricao,
                            CssClass = y.CssClass,
                            CssIcon = y.CssIcon,
                            ToolTip = y.ToolTip
                        }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(StatusFila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusFila.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(StatusFila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.StatusFila.Update(Entity);
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
