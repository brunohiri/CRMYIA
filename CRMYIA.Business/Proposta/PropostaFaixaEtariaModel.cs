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
    public class PropostaFaixaEtariaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static PropostaFaixaEtaria Get(long IdPropostaFaixaEtaria)
        {
            PropostaFaixaEtaria Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.PropostaFaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdPropostaFaixaEtaria == IdPropostaFaixaEtaria)
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

        public static PropostaFaixaEtaria Get(long IdProposta, byte IdFaixaEtaria)
        {
            PropostaFaixaEtaria Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.PropostaFaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProposta == IdProposta && x.IdFaixaEtaria == IdFaixaEtaria)
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

        public static List<PropostaFaixaEtaria> GetList()
        {
            List<PropostaFaixaEtaria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.PropostaFaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdPropostaFaixaEtaria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<PropostaFaixaEtaria> GetList(long IdProposta)
        {
            List<PropostaFaixaEtaria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.PropostaFaixaEtaria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProposta == IdProposta)
                        .AsNoTracking()
                        .OrderBy(o => o.IdPropostaFaixaEtaria).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(PropostaFaixaEtaria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.PropostaFaixaEtaria.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddRange(List<PropostaFaixaEtaria> ListEntity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.PropostaFaixaEtaria.AddRange(ListEntity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(PropostaFaixaEtaria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.PropostaFaixaEtaria.Update(Entity);
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
