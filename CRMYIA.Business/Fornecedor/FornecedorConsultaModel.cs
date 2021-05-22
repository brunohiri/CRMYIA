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
    public class FornecedorConsultaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static FornecedorConsulta Get(long IdFornecedorConsulta)
        {
            FornecedorConsulta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.FornecedorConsulta
                        .AsNoTracking()
                        .Where(x => x.IdFornecedorConsulta == IdFornecedorConsulta)
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

        public static List<FornecedorConsulta> GetList()
        {
            List<FornecedorConsulta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FornecedorConsulta
                        .Include(y=>y.IdFornecedorNavigation)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataConsulta).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(FornecedorConsulta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FornecedorConsulta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(FornecedorConsulta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FornecedorConsulta.Update(Entity);
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
