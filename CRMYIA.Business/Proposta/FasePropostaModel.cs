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
    public class FasePropostaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static FaseProposta Get(long IdFaseProposta)
        {
            FaseProposta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.FaseProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdFaseProposta == IdFaseProposta)
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

        public static List<FaseProposta> GetList()
        {
            List<FaseProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FaseProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdFaseProposta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<FaseProposta> GetListIdDescricao()
        {
            List<FaseProposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FaseProposta
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new FaseProposta()
                        {
                            IdFaseProposta = y.IdFaseProposta,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdFaseProposta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(FaseProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FaseProposta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(FaseProposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FaseProposta.Update(Entity);
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
