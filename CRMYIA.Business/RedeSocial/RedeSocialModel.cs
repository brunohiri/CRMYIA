using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CRMYIA.Business
{
    public class RedeSocialModel
    {

        public static RedeSocial Get(long IdRedeSocial)
        {
            RedeSocial Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.RedeSocial
                        .Where(x => x.IdRedeSocial == IdRedeSocial)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<RedeSocial> GetList()
        {
            List<RedeSocial> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.RedeSocial
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .OrderBy(o => o.Nome).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static void Add(RedeSocial Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.RedeSocial.Add(Entity);
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
