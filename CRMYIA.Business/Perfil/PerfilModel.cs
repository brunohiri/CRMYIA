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
    public class PerfilModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Perfil Get(long IdPerfil)
        {
            Perfil Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Perfil
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdPerfil == IdPerfil)
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

        public static List<Perfil> GetList()
        {
            List<Perfil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Perfil
                        .Include(y => y.UsuarioPerfil)
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Perfil> GetDescricao(long IdUsuario)
        {
            List<Perfil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Perfil
                        .Join(context.UsuarioPerfil, p => p.IdPerfil, up => up.IdPerfil, (p, up) => new { p, up })
                        .Select(x => new Perfil()
                        {
                            IdPerfil = x.p.IdPerfil,
                            Descricao = x.p.Descricao
                        })
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;

        }
            public static List<PerfilViewModel> GetListIdDescricao(long IdUsuario, ref byte? IdPerfil)
        {
            List<PerfilViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {

                   UsuarioPerfil up = context.UsuarioPerfil.Find(IdUsuario);
                   IdPerfil = up.IdPerfil;

                    if (up.IdPerfil == 1)
                    {
                        ListEntity = context.Perfil
                        .Select(x => new PerfilViewModel()
                        {
                            IdPerfil = x.IdPerfil,
                            Descricao = x.Descricao
                        })
                        .Where(o => o.IdPerfil != up.IdPerfil && o.IdPerfil != 1)
                        .ToList();
                    }

                    if (up.IdPerfil == 2)
                    {
                        ListEntity = context.Perfil
                       .Select(x => new PerfilViewModel()
                       {
                           IdPerfil = x.IdPerfil,
                           Descricao = x.Descricao
                       })
                       .Where(o => o.IdPerfil != up.IdPerfil && o.IdPerfil != 1 && o.IdPerfil != 2)
                        .ToList();
                    }

                    if (up.IdPerfil == 3)
                    {
                        ListEntity = context.Perfil
                        .Select(x => new PerfilViewModel()
                        {
                            IdPerfil = x.IdPerfil,
                            Descricao = x.Descricao
                        })
                        .Where(o => o.IdPerfil != up.IdPerfil && o.IdPerfil != 1 && o.IdPerfil != 2 && o.IdPerfil != 3)
                        .ToList();
                    }

                    if (up.IdPerfil == 4)
                    {
                        ListEntity = context.Perfil
                         .Select(x => new PerfilViewModel()
                         {
                             IdPerfil = x.IdPerfil,
                             Descricao = x.Descricao
                         })
                         .Where(o => o.IdPerfil != up.IdPerfil && o.IdPerfil != 1 && o.IdPerfil != 2 && o.IdPerfil != 3 && o.IdPerfil != 4)
                        .ToList();
                    }

                    //ListEntity = context.Perfil
                    //     .Select(x => new Perfil()
                    //     {
                    //         IdPerfil = x.IdPerfil,
                    //         Descricao = x.Descricao
                    //     })
                    //     .Where(o => o.IdPerfil != up.IdPerfil)
                    //     .ToList();


                    //ListEntity = context.Perfil
                    //   .Join(context.UsuarioPerfil, p => p.IdPerfil, up => up.IdPerfil, (p, up) => new { p, up })
                    //   .Join(context.Usuario, up => up.up.IdUsuario, u => u.IdUsuario, (up, u) => new { up, u })
                    //   .Select(x => new Perfil()
                    //   {
                    //        IdPerfil = x.up.p.IdPerfil,
                    //        Descricao = x.up.p.Descricao
                    //   })
                    //    .AsNoTracking()
                    //    .Where(x => x.Ativo)
                    //    .AsNoTracking()
                    //   .OrderBy(o => o.Descricao).ToList();

                    //Usuario u = context.Usuario.Find(IdUsuario);
                    //UsuarioPerfil up = context.UsuarioPerfil.Find(u.IdUsuario);

                    //if (u != null && up != null)
                    //{
                    //    ListEntity = context.Perfil
                    //             .AsNoTracking()
                    //             .Where(x => x.Ativo && x.IdPerfil == u.IdUsuario && x.IdPerfil == up.IdPerfil)
                    //             .AsNoTracking()
                    //             .Select(y => new Perfil()
                    //             {
                    //                 IdPerfil = y.IdPerfil,
                    //                 Descricao = y.Descricao
                    //             }).OrderBy(o => o.Descricao).ToList();
                    //}


                    //ListEntity = context.Perfil
                    //    .AsNoTracking()
                    //    .Where(x => x.Ativo)
                    //    .AsNoTracking()
                    //    .Select(y => new Perfil()
                    //    {
                    //        IdPerfil = y.IdPerfil,
                    //        Descricao = y.Descricao
                    //    }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Perfil> GetListIdDescricao()
        {
            List<Perfil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Perfil
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Perfil()
                        {
                            IdPerfil = y.IdPerfil,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        
        public static void Add(Perfil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Perfil.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Perfil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Perfil.Update(Entity);
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
