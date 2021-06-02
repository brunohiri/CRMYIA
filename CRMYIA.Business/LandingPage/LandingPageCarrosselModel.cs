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
    public class LandingPageCarrosselModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos

        public static LandingPageCarrossel Get(long IdLandingPageCarrossel)
        {
            LandingPageCarrossel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.LandingPageCarrossel.Include(x => x.IdUsuarioNavigation)
                        .Where(x => x.IdLandingPageCarrossel == IdLandingPageCarrossel && x.Ativo)
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
        public static List<LandingPageCarrossel> GetList()
        {
            List<LandingPageCarrossel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.LandingPageCarrossel.Include(x => x.IdUsuarioNavigation).Where(x => x.Ativo)
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
        public static List<LandingPageCarrossel> GetList(long IdUsuario)
        {
            List<LandingPageCarrossel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.LandingPageCarrossel.Include(x => x.IdUsuarioNavigation).Where(x => x.IdUsuario == IdUsuario && x.Ativo)
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
        public static void Add(LandingPageCarrossel Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LandingPageCarrossel.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(LandingPageCarrossel Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.LandingPageCarrossel.Update(Entity);
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