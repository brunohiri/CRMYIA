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
    public class CategoriaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Categoria Get(long IdCategoria)
        {
            Categoria Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Categoria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdCategoria == IdCategoria)
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

        public static List<Categoria> GetList()
        {
            List<Categoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Categoria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdCategoria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Categoria> GetListIdDescricao()
        {
            List<Categoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Categoria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Categoria()
                        {
                            IdCategoria = y.IdCategoria,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdCategoria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Categoria> GetListIdDescricaoByOperadora(long IdOperadora)
        {
            List<Categoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Categoria
                        .Include(y => y.IdLinhaNavigation)
                        .ThenInclude(k => k.IdProdutoNavigation)
                        .Where(x => x.Ativo && x.IdLinhaNavigation.IdProdutoNavigation.IdOperadora == IdOperadora)
                        .AsNoTracking()
                        .Select(y => new Categoria()
                        {
                            IdCategoria = y.IdCategoria,
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

        public static List<Categoria> GetListIdDescricaoByProduto(long IdProduto)
        {
            List<Categoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Categoria
                        .Include(y => y.IdLinhaNavigation)
                        .Where(x => x.Ativo && x.IdLinhaNavigation.IdProduto == IdProduto)
                        .AsNoTracking()
                        .Select(y => new Categoria()
                        {
                            IdCategoria = y.IdCategoria,
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

        public static List<Categoria> GetListIdDescricaoByLinha(long IdLinha)
        {
            List<Categoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Categoria
                        .Where(x => x.Ativo && x.IdLinha == IdLinha)
                        .AsNoTracking()
                        .Select(y => new Categoria()
                        {
                            IdCategoria = y.IdCategoria,
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

        public static void Add(Categoria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Categoria.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Categoria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Categoria.Update(Entity);
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
