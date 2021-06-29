using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business.YNDICA
{
    public class FilaItemModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static FilaItem Get(long IdFilaItem)
        {
            FilaItem Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.FilaItem
                        .Include(z => z.IdFornecedorConsultaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdFilaItem == IdFilaItem)
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

        public static List<FilaItem> GetList()
        {
            List<FilaItem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FilaItem
                        .Include(z => z.IdFornecedorConsultaNavigation)
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

        public static List<FilaItem> GetList(bool Processado)
        {
            List<FilaItem> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.FilaItem
                        .Include(z => z.IdFornecedorConsultaNavigation)
                        .Where(x => x.Processado == Processado)
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

        public static void Add(FilaItem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FilaItem.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(FilaItem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.FilaItem.Update(Entity);
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
