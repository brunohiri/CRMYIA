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
    public class ClienteModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Cliente Get(long IdCliente)
        {
            Cliente Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(y => y.IdOrigemNavigation)
                        .Include(y => y.IdTipoLeadNavigation)
                        .Include(y => y.IdEstadoCivilNavigation)
                        .Include(y => y.IdGeneroNavigation)
                        .Include(t => t.Telefone)
                        .Include(e => e.Email)
                        .Where(x => x.IdCliente == IdCliente)
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

        public static Cliente GetByCPF(string Cpf = null)
        {
            Cliente Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Where(x => x.CPF == Cpf)
                        //?.AsNoTracking()
                        ?.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Cliente> GetList()
        {
            List<Cliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(y => y.IdOrigemNavigation)
                        .Include(y => y.IdTipoLeadNavigation)
                        .Include(y => y.IdEstadoCivilNavigation)
                        .Include(y => y.IdGeneroNavigation)
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Cliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cliente.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Cliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cliente.Update(Entity);
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
