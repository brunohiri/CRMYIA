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
    public class UsuarioModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Task<Usuario> Get(long IdUsuario)
        {
            Task<Usuario> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Task<List<Usuario>> GetList()
        {
            Task<List<Usuario>> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                        .AsNoTracking()
                        .ToListAsync(); 
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Usuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Usuario.AddAsync(Entity);
                    context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Usuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Usuario.Update(Entity);
                    context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Outros Métodos
        public static Usuario Autenticar(string LoginEmail, string LoginSenha, string Ip)
        {
            Usuario EntityUsuario = null;
            using (YiaContext context = new YiaContext())
            {
                EntityUsuario = context.Usuario
                    .Include(y => y.UsuarioPerfil)
                    .Include(y => y.IdCorretoraNavigation)
                    .Where(x => x.Login == LoginEmail && x.Senha == Criptography.Encrypt(LoginSenha)).FirstOrDefault();
            }

            return EntityUsuario;

        }
        #endregion
    }
}
