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
    public class AbordagemModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Abordagem Get(long IdAbordagem)
        {
            Abordagem Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Abordagem
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdAbordagem == IdAbordagem)
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

        public static Abordagem GetNext(byte IdAbordagemCategoria, byte Ordem)
        {
            Abordagem Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Abordagem
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdAbordagemCategoria == IdAbordagemCategoria && x.Ordem == Ordem + 1)
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

        public static Abordagem GetPrevious(byte IdAbordagemCategoria, byte Ordem)
        {
            Abordagem Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Abordagem
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdAbordagemCategoria == IdAbordagemCategoria && x.Ordem == Ordem - 1)
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

        public static List<Abordagem> GetList()
        {
            List<Abordagem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Abordagem
                         .AsNoTracking()
                         .Where(x => x.Ativo)
                         .AsNoTracking()
                         .OrderBy(o=>o.Ordem)
                         .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Abordagem> GetList(byte IdAbordagemCategoria)
        {
            List<Abordagem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Abordagem
                         .AsNoTracking()
                         .Where(x => x.Ativo && x.IdAbordagemCategoria == IdAbordagemCategoria)
                         .AsNoTracking()
                         .OrderBy(o => o.Ordem)
                         .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Abordagem> GetListIdDescricao()
        {
            List<Abordagem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Abordagem
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Abordagem()
                        {
                            IdAbordagem = y.IdAbordagem,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.Ordem).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Abordagem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Abordagem.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Abordagem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Abordagem.Update(Entity);
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
