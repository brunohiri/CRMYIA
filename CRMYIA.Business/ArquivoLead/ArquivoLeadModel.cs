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
    public class ArquivoLeadModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static ArquivoLead Get(long IdArquivoLead)
        {
            ArquivoLead Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.ArquivoLead
                        .Where(x => x.IdArquivoLead == IdArquivoLead)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static ArquivoLead GetLastId()
        {
            ArquivoLead Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.ArquivoLead
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

        public static List<ArquivoLead> GetList()
        {
            List<ArquivoLead> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.ArquivoLead
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

        public static void Add(ArquivoLead Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.ArquivoLead.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(ArquivoLead Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.ArquivoLead.Update(Entity);
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
