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
    public class FaixaEtariaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static FaixaEtaria Get(long IdFaixaEtaria)
        {
            FaixaEtaria Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.FaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdFaixaEtaria == IdFaixaEtaria)
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

        public static List<FaixaEtaria> GetList()
        {
            List<FaixaEtaria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdFaixaEtaria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<FaixaEtaria> GetListIdDescricao()
        {
            List<FaixaEtaria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new FaixaEtaria()
                        {
                            IdFaixaEtaria = y.IdFaixaEtaria,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdFaixaEtaria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(FaixaEtaria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FaixaEtaria.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddRange(List<FaixaEtaria> ListEntity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FaixaEtaria.AddRange(ListEntity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(FaixaEtaria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FaixaEtaria.Update(Entity);
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
