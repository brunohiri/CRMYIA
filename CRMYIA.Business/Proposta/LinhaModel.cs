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
    public class LinhaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Linha Get(long IdLinha)
        {
            Linha Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Linha
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdLinha == IdLinha)
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

        public static List<Linha> GetList()
        {
            List<Linha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Linha
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdLinha).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Linha> GetListIdDescricao()
        {
            List<Linha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Linha
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Linha()
                        {
                            IdLinha = y.IdLinha,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdLinha).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Linha> GetListIdDescricaoByOperadora(long IdOperadora)
        {
            List<Linha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Linha
                        .AsNoTracking()
                        .Include(y=>y.IdProdutoNavigation)
                        .Where(x => x.Ativo && x.IdProdutoNavigation.IdOperadora == IdOperadora)
                        .AsNoTracking()
                        .Select(y => new Linha()
                        {
                            IdLinha = y.IdLinha,
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

        public static List<Linha> GetListIdDescricaoByProduto(long IdProduto)
        {
            List<Linha> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Linha
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProduto == IdProduto)
                        .AsNoTracking()
                        .Select(y => new Linha()
                        {
                            IdLinha = y.IdLinha,
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

        public static void Add(Linha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Linha.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Linha Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Linha.Update(Entity);
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
