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
    public class HistoricoLigacaoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static HistoricoLigacao Get(long IdHistoricoLigacao)
        {
            HistoricoLigacao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.HistoricoLigacao
                        .AsNoTracking()
                        .Where(x =>x.IdHistoricoLigacao == IdHistoricoLigacao)
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

        public static List<HistoricoLigacao> GetList()
        {
            List<HistoricoLigacao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.HistoricoLigacao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdHistoricoLigacao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<HistoricoLigacao> GetList(string IdProposta)
        {
            List<HistoricoLigacao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.HistoricoLigacao
                        .Include(y => y.IdUsuarioNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdProposta == IdProposta.ExtractLong())
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

        public static void Add(HistoricoLigacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoLigacao.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(HistoricoLigacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.HistoricoLigacao.Update(Entity);
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
