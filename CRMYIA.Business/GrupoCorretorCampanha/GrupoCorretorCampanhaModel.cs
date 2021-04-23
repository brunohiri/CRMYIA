using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class GrupoCorretorCampanhaModel
    {


        public static List<GrupoCorretorCampanha> Get(long IdCampanha)
        {
            List<GrupoCorretorCampanha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.GrupoCorretorCampanha
                        .Include(x=>x.IdCampanhaNavigation)// Rever
                            //.ThenInclude(x=>x.RedeSocial)
                            //    .ThenInclude(x=>x.CapaRedeSocial)
                            //        .ThenInclude(x=>x.IdCapaNavigation)
                        .Where(x => x.IdCampanha == IdCampanha)
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

        public static List<Campanha> GetListLinks(long IdCampanha)
        {
            List<Campanha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Campanha
                        .Include(ca => ca.CampanhaArquivo)
                        .Include(gcc => gcc.GrupoCorretorCampanha)
                        .Include(v => v.Video)
                        .Include(crs => crs.CapaRedeSocial)
                            .ThenInclude(c=>c.IdCapaNavigation)
                        .Include(ac => ac.AssinaturaCartao)
                        .Include(b => b.Banner)
                        .Where(x => x.IdCampanha == IdCampanha)
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<GrupoCorretorCampanha> Get(byte IdGrupoCorretor)
        {
            List<GrupoCorretorCampanha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.GrupoCorretorCampanha
                        .Where(x => x.IdGrupoCorretor == IdGrupoCorretor)
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

        public static void Add(GrupoCorretorCampanha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    
                    context.GrupoCorretorCampanha.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(GrupoCorretorCampanha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    
                    context.GrupoCorretorCampanha.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Delete(GrupoCorretorCampanha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {

                    context.GrupoCorretorCampanha.Remove(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
