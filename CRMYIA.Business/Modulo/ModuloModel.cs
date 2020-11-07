using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class ModuloModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Modulo Get(long IdModulo)
        {
            Modulo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Modulo
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdModulo == IdModulo)
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

        public static List<Modulo> GetList()
        {
            List<Modulo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modulo
                                     .Include(y=>y.IdModuloReferenciaNavigation)
                                     .AsNoTracking()
                                     .OrderBy(x => x.Ordem)
                                     .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Modulo> GetList(long IdUsuario)
        {
            List<Modulo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modulo
                                    .Where(m => m.Ativo && m.PerfilModulo
                                        .Where(pm => pm.Ativo && pm.IdPerfilNavigation.UsuarioPerfil
                                            .Where(up => up.Ativo && up.IdUsuario == IdUsuario
                                            ).Count() > 0
                                         ).Count() > 0
                                     ).AsNoTracking()
                                     .OrderBy(x => x.Ordem)
                                     .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Modulo> GetListIdDescricao()
        {
            List<Modulo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modulo
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Modulo()
                        {
                            IdModulo = y.IdModulo,
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

        public static bool PermitirAcesso(long IdUsuario, long IdModulo)
        {
            List<Modulo> ListEntity = null;
            bool AcessoPermitido = false;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modulo
                                    .Where(m => m.Ativo && m.IdModulo == IdModulo && m.PerfilModulo
                                        .Where(pm => pm.Ativo && pm.IdPerfilNavigation.UsuarioPerfil
                                            .Where(up => up.Ativo && up.IdUsuario == IdUsuario
                                            ).Count() > 0
                                         ).Count() > 0
                                     ).AsNoTracking().ToList();

                    AcessoPermitido = ((ListEntity != null) && (ListEntity.Count() > 0));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return AcessoPermitido;
        }

        public static void Add(Modulo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Modulo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Modulo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Modulo.Update(Entity);
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
