using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class CapaRedeSocialModel
    {
        
        public static CapaRedeSocial Get(long IdCapa)
        {
            CapaRedeSocial Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CapaRedeSocial
                        .Where(x => x.IdCapa == IdCapa)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static void Add(CapaRedeSocial Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CapaRedeSocial.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static void Update(CapaRedeSocial Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CapaRedeSocial.Update(Entity);
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
