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
    public class CidadeModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Cidade Get(long IdCidade)
        {
            Cidade Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cidade
                        .Include(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdCidade == IdCidade)
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

        public static Cidade Get(string CodigoIBGE)
        {
            Cidade Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cidade
                        .Include(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdCidade == CodigoIBGE.ExtractInt32OrNull())
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

        public static Cidade GetByDescricao(string Descricao)
        {
            Cidade Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cidade
                        .Include(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.Descricao.ToUpper() == Descricao.RemoverAcentuacao().ToUpper())
                        .AsEnumerable()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Cidade> GetList()
        {
            List<Cidade> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cidade
                       .Include(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.Descricao)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Cidade> GetListIdDescricao()
        {
            List<Cidade> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cidade
                        .AsNoTracking()
                        .Select(y => new Cidade()
                        {
                            IdCidade = y.IdCidade,
                            Descricao = y.Descricao,
                            CodigoIBGE = y.CodigoIBGE
                        }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Cidade Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cidade.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Cidade Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cidade.Add(Entity);
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
