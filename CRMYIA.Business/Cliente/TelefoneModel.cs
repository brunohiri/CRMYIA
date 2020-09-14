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
    public class TelefoneModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Telefone Get(long IdTelefone)
        {
            Telefone Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Telefone
                        .AsNoTracking()
                        .Where(x => x.IdTelefone == IdTelefone)
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


        public static List<Telefone> GetList()
        {
            List<Telefone> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Telefone
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.DataCadastro).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Telefone> GetList(long IdCliente)
        {
            List<Telefone> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Telefone
                        .Include(y => y.IdOperadoraTelefoneNavigation)
                        .Where(x => x.IdCliente == IdCliente)
                        .AsNoTracking()
                        .OrderBy(o => o.DataCadastro).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Telefone Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Telefone.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Telefone Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Telefone.Update(Entity);
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
