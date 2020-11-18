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
    public class OrigemLeadModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Origem Get(long IdOrigem)
        {
            Origem Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Origem
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdOrigem == IdOrigem)
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

        public static Origem GetByCode(long code)
        {
            Origem Entity = null;
            try
            {
                long IdOrigem = code == 1023 ? 2 : 0;
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Origem
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdOrigem == IdOrigem)
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

        public static List<Origem> GetList()
        {
            List<Origem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Origem
                        .Include(y => y.Cliente)
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

        public static List<Origem> GetListIdDescricao()
        {
            List<Origem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Origem
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Origem()
                        {
                            IdOrigem = y.IdOrigem,
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

        public static void Add(Origem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Origem.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Origem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Origem.Update(Entity);
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
