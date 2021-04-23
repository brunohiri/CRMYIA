using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Business.Dashboard;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class OperadoraModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Operadora Get(long IdOperadora)
        {
            Operadora Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Operadora
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdOperadora == IdOperadora)
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

        public static List<Operadora> GetList()
        {
            List<Operadora> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Operadora
                        .Include(y => y.Produto)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Banner> GetListaOperadora(long IdCampanha, byte IdGrupoCorretor)
        {
            //List<AssinaturaCartaoViewModel> ListEntity = new List<CapaViewModel>();
            List<Banner> ListBanner = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListBanner = context.Banner
                    .Include(x => x.IdCampanhaNavigation)
                        .ThenInclude(x => x.GrupoCorretorCampanha)
                    .Where(x => x.IdCampanha == IdCampanha && x.IdCampanhaNavigation.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == IdGrupoCorretor).Count() > 0)
                    .AsNoTracking()
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListBanner;
        }

        public static List<Operadora> GetListIdDescricao()
        {
            List<Operadora> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Operadora
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Operadora()
                        {
                            IdOperadora = y.IdOperadora,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static Operadora GetLastId()
        {
            Operadora Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Operadora
                        .ToList()
                        .LastOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static void Add(Operadora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Operadora.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Operadora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Operadora.Update(Entity);
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
