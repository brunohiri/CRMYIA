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
        public static void DesativarUsuarioSemProdução()
        {
            List<UsuarioPerfil> Entity = null;
            DateTime hoje = DateTime.Now;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.UsuarioPerfil.Where(x => x.IdPerfil == (decimal)EnumeradorModel.Perfil.Corretor && x.Ativo == true).AsNoTracking().ToList();

                    foreach (var user in Entity)
                    {
                        var proposta = context.Proposta.Where(y => y.IdUsuario == user.IdUsuario && y.DataSolicitacao >= hoje.AddMonths(-3) && y.DataSolicitacao <= hoje && y.Ativo == true).ToList();

                        if(proposta.Count > 0)
                        {
                            foreach(var item in proposta)
                            {
                                var userDesativar = context.Usuario.Where(i => i.IdUsuario == item.IdUsuarioCorretor).FirstOrDefault();
                                userDesativar.Ativo = false;

                                context.Usuario.Update(userDesativar);
                                context.SaveChanges();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        public static List<UsuarioViewModel> GetList(byte IdPerfil)
        {
            List<UsuarioViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioPerfil
                        .Where(x => x.IdPerfil == IdPerfil && x.Ativo == true)
                        .AsNoTracking()
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(z => z.IdUsuarioNavigation.PropostaIdUsuarioCorretorNavigation)
                        .Select(c => new UsuarioViewModel
                        {
                            IdUsuario = c.IdUsuario.Value,
                            Nome = c.IdUsuarioNavigation.Nome
                        }).ToList();
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
