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
    public class CorretoraModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Corretora Get(long IdCorretora)
        {
            Corretora Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Corretora
                        .Include(y => y.IdCidadeNavigation)
                        .ThenInclude(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdCorretora == IdCorretora)
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

        public static List<Corretora> GetList()
        {
            List<Corretora> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Corretora
                        .Include(y => y.IdCidadeNavigation)
                        .ThenInclude(p => p.IdEstadoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.RazaoSocial).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Corretora> GetListIdDescricao()
        {
            List<Corretora> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Corretora
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Corretora()
                        {
                            IdCorretora = y.IdCorretora,
                            RazaoSocial = y.RazaoSocial,
                            NomeFantasia = y.NomeFantasia
                        }).OrderBy(o => o.RazaoSocial).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Corretora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity.IdCidade = CidadeModel.Get(Entity.IdCidade.ToString()).IdCidade;
                    context.Corretora.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Corretora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity.IdCidade = CidadeModel.Get(Entity.IdCidade.ToString()).IdCidade;
                    context.Corretora.Update(Entity);
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
