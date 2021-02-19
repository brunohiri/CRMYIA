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
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class CampanhaArquivoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static CampanhaArquivo Get(long IdArquivo)
        {
            CampanhaArquivo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CampanhaArquivo
                        .Where(x => x.IdCampanhaArquivo == IdArquivo)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static CampanhaArquivo GetLastId()
        {
            CampanhaArquivo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CampanhaArquivo
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

        public static List<CampanhaArquivo> GetListaCampanhaArquivo()
        {
            List<CampanhaArquivo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .OrderBy(o => o.IdCampanha)
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
        public static List<CampanhaArquivo> GetListaCampanhaArquivo(long Id)
        {
            List<CampanhaArquivo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .Where(x => x.IdCampanhaArquivo == Id)
                        .OrderBy(o => o.Descricao)
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

        public static List<CampanhaArquivo> GetList()
        {
            List<CampanhaArquivo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .AsNoTracking()
                        .OrderByDescending(o => o.IdCampanha).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(CampanhaArquivo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CampanhaArquivo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(CampanhaArquivo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CampanhaArquivo.Update(Entity);
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
