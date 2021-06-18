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
        #region Métodos
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
        public static GrupoCorretor Get(long IdGrupoCorretor)
        {
            GrupoCorretor Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.GrupoCorretor
                        .Where(x => x.Ativo && x.IdGrupoCorretor == IdGrupoCorretor)
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
        public static void Add(GrupoCorretor Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.GrupoCorretor.Add(Entity);
                    context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(GrupoCorretor Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.GrupoCorretor.Update(Entity);
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
