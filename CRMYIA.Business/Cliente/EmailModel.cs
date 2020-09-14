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
    public class EmailModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Email Get(long IdEmail)
        {
            Email Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Email
                        .AsNoTracking()
                        .Where(x => x.IdEmail == IdEmail)
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


        public static List<Email> GetList()
        {
            List<Email> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Email
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.EmailConta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Email> GetList(long IdCliente)
        {
            List<Email> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Email
                        .Where(x => x.IdCliente == IdCliente)
                        .AsNoTracking()
                        .OrderBy(o => o.EmailConta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Email Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Email.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Email Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Email.Update(Entity);
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
