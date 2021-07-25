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
    public class UsuarioHierarquiaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static UsuarioHierarquia Get(long IdUsuario)
        {
            UsuarioHierarquia Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.UsuarioHierarquia
                        .Where(x => x.IdUsuarioMaster == IdUsuario)
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

        public static UsuarioHierarquia GetSlave(long IdUsuario)
        {
            UsuarioHierarquia Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.UsuarioHierarquia
                        .Where(x => x.IdUsuarioSlave == IdUsuario)
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
        public static UsuarioHierarquia GetMaster(long IdUsuario)
        {
            UsuarioHierarquia Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.UsuarioHierarquia.Include(x => x.IdUsuarioMasterNavigation)
                        .Where(x => x.IdUsuarioSlave == IdUsuario)
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
        public static List<UsuarioHierarquia> GetList()
        {
            List<UsuarioHierarquia> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioHierarquia
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

        public static List<UsuarioHierarquia> GetAllUsuarioMaster()
        {
            List<UsuarioHierarquia> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioHierarquia
                        .Include(x => x.IdUsuarioMasterNavigation)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(UsuarioHierarquia Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioHierarquia.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(UsuarioHierarquia Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioHierarquia.Update(Entity);
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
