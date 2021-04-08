using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class GrupoCorretorModel
    {
        public static List<GrupoCorretor> GetList()
        {
            List<GrupoCorretor> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.GrupoCorretor
                        .Include(x => x.GrupoCorretorCampanha)
                        .Where(x => x.Ativo)
                        .OrderBy(o => o.Descricao)
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
    }
}
