using CRMYIA.Data.Context;
using System;
using CRMYIA.Data.Entities;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class CampanhaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        
        public static List<Campanha> GetList()
        {
            List<Campanha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Campanha
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
        public static List<Campanha> GetListOrderById()
        {
            List<Campanha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Campanha
                        .OrderBy(o => o.IdCampanha)
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

        public static Campanha Get(long Id)
        {
            Campanha Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Campanha
                        .Where(x => x.IdCampanha == Id)
                        .FirstOrDefault();
                        
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static void Add(Campanha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Campanha.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Campanha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Campanha.Update(Entity);
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
