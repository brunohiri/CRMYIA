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
    public class UsuarioPerfilModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static UsuarioPerfil Get(long IdUsuario)
        {
            UsuarioPerfil Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.UsuarioPerfil
                        .Where(x => x.IdUsuario == IdUsuario)
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

        public static List<UsuarioPerfil> GetList()
        {
            List<UsuarioPerfil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioPerfil
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

        public static List<UsuarioPerfil> GetList(byte IdPerfil)
        {
            List<UsuarioPerfil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioPerfil
                        .Include(y => y.IdUsuario)
                        .AsNoTracking()
                        .Where(z => z.IdPerfil == IdPerfil)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(UsuarioPerfil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioPerfil.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(UsuarioPerfil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioPerfil.Update(Entity);
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
