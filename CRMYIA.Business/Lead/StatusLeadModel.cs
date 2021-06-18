using System;
using System.Collections.Generic;
using System.Linq;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business.Lead
{
    public class StatusLeadModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static StatusLead Get(long IdStatusLead)
        {
            try
            {
                using YiaContext context = new YiaContext();
                return context.StatusLead
                    .AsNoTracking()
                    .Where(x => x.Ativo && x.IdStatusLead == IdStatusLead)
                    .AsNoTracking()
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<StatusLead> GetList()
        {
            try
            {
                using YiaContext context = new YiaContext();
                return context.StatusLead
                    .AsNoTracking()
                    .Where(x => x.Ativo)
                    .AsNoTracking()
                    .OrderBy(o => o.IdStatusLead).ToList();
            } catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
