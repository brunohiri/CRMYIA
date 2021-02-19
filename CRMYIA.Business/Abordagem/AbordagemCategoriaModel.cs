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
    public class AbordagemCategoriaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static AbordagemCategoria Get(long IdAbordagemCategoria)
        {
            AbordagemCategoria Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.AbordagemCategoria
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdAbordagemCategoria == IdAbordagemCategoria)
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

        public static List<AbordagemCategoria> GetList()
        {
            List<AbordagemCategoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    //ListEntity = context.AbordagemCategoria
                    //    .Where(x => x.Ativo)
                    //    .Include(KPICargo => KPICargo.IdKPICargoNavigation)
                    //    .Include(KPIServico => KPIServico.IdKPIServicoNavigation)
                    //    .AsNoTracking()
                    //    .OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<AbordagemCategoria> GetListIdDescricao()
        {
            List<AbordagemCategoria> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.AbordagemCategoria
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new AbordagemCategoria()
                        {
                            IdAbordagemCategoria = y.IdAbordagemCategoria,
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

        public static void Add(AbordagemCategoria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.AbordagemCategoria.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(AbordagemCategoria Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.AbordagemCategoria.Update(Entity);
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
