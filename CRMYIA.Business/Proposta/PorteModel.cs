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
    public class PorteModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Porte Get(long IdPorte)
        {
            Porte Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Porte
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdPorte == IdPorte)
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

        public static List<Porte> GetList()
        {
            List<Porte> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Porte
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdPorte).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Porte> GetListIdDescricao()
        {
            List<Porte> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Porte
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Porte()
                        {
                            IdPorte = y.IdPorte,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdPorte).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Porte Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Porte.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Porte Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Porte.Update(Entity);
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
