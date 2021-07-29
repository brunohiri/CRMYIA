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
    public class MotivoDeclinioLeadModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static async Task<MotivoDeclinioLead> GetAsync(long id)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    return await context.MotivoDeclinioLead
                        .AsNoTracking()
                        .Where(x => x.IdMotivoDeclinioLead == id && x.Ativo)
                        .FirstOrDefaultAsync();
                }
            } 
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<List<MotivoDeclinioLead>> GetListAsync()
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    return await context.MotivoDeclinioLead
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .OrderBy(o => o.IdMotivoDeclinioLead)
                        .ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
