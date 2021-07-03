using CRMYIA.Business.ShiftData;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYIA.Business.YNDICA
{
    public class FilaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Fila Get(long IdFila)
        {
            Fila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.FilaItem)
                        .ThenInclude(z => z.IdFornecedorConsultaNavigation)
                        .Where(x => x.IdFila == IdFila)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Fila GetUpdate(long IdFila)
        {
            Fila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Fila
                        .Where(x => x.IdFila == IdFila)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Fila GetLastId()
        {
            Fila Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Fila
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

        public static List<Fila> GetList()
        {
            List<Fila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.IdFornecedorNavigation)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataEntrada).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Fila> GetListByStatusFila(short IdStatusFila)
        {
            List<Fila> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Fila
                        .Include(y => y.IdStatusFilaNavigation)
                        .Include(y => y.IdLayoutNavigation)
                        .Include(y => y.FilaItem)
                        .ThenInclude(z => z.IdFornecedorConsultaNavigation)
                        .Where(x => x.IdStatusFila == IdStatusFila)
                        .OrderByDescending(o => o.DataEntrada)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Fila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Fila.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Fila Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Fila.Update(Entity);
                    //context.Attach(Entity);
                    //context.Entry(Entity).Property(p => p.IdStatusFila).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Outros Métodos
       
        #endregion
    }
}
