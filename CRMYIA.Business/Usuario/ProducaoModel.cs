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
    public class ProducaoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Producao Get(long IdProducao)
        {
            Producao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Producao
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProducao == IdProducao)
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

        public static List<Producao> GetList()
        {
            List<Producao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Producao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdProducao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Producao> GetListIdDescricao()
        {
            List<Producao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Producao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Producao()
                        {
                            IdProducao = y.IdProducao,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdProducao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Producao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Producao.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Producao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Producao.Update(Entity);
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
