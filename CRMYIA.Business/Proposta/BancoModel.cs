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
    public class BancoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Banco Get(long IdBanco)
        {
            Banco Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Banco
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdBanco == IdBanco)
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

        public static List<Banco> GetList()
        {
            List<Banco> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Banco
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdBanco).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Banco> GetListIdDescricao()
        {
            List<Banco> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Banco
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Banco()
                        {
                            IdBanco = y.IdBanco,
                            Codigo = y.Codigo,
                            Nome = y.Nome
                        }).OrderBy(o => o.Nome).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Banco Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Banco.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Banco Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Banco.Update(Entity);
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
