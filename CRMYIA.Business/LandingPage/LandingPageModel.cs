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
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class LandingPageModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
                        
        public static LandingPage Get(long IdLandingPage)
        {
            LandingPage Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.LandingPage
                        .Where(x => x.IdLandingPage == IdLandingPage)
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
        public static List<LandingPage> GetList()
        {
            List<LandingPage> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.LandingPage
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
        public static void Add(LandingPage Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LandingPage.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(LandingPage Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LandingPage.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Outros Métodos
        
        #endregion
    }
}