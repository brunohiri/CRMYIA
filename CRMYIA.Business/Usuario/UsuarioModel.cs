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
        public static Usuario Get(long IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
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

        public static Usuario GetByCPF(string Cpf = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.CPF == Cpf)
                        //?.AsNoTracking()
                        ?.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Usuario GetByEmail(string Email = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.Email.ToLower() == Email.ToLower())
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

        public static Usuario GetByLogin(string Login = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.Login.ToLower() == Login.ToLower())
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

        public static Usuario GetUsuariosMaster(long IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Include(y=>y.UsuarioHierarquiaIdUsuarioMasterNavigation)
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

        public static Usuario GetUsuariosSlave(long IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Include(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
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

        public static List<Usuario> GetList()
        {
            List<Usuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                        .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
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

        public static List<Usuario> GetList(byte IdPerfil)
        {
            List<Usuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                        .AsNoTracking()
                        .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil))
                        .ToList();
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
                    Entity.Senha = Criptography.Encrypt(Entity.Senha);
                    context.Usuario.Add(Entity);
                    context.SaveChanges();
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
