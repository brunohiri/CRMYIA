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
    public class PropostaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Proposta Get(long IdProposta)
        {
            Proposta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Proposta
                        .AsNoTracking()
                        .Where(x =>x.IdProposta == IdProposta)
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

        public static List<Proposta> GetList()
        {
            List<Proposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Proposta
                        .Include(y => y.IdModalidadeNavigation)
                        .Include(y => y.IdFasePropostaNavigation)
                        .Include(y => y.IdStatusPropostaNavigation)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(y => y.IdClienteNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataCadastro).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Proposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Proposta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Proposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Proposta.Update(Entity);
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
