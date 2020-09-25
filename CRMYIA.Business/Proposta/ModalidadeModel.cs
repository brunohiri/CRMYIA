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
    public class ModalidadeModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Modalidade Get(long IdModalidade)
        {
            Modalidade Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Modalidade
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdModalidade == IdModalidade)
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

        public static List<Modalidade> GetList()
        {
            List<Modalidade> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modalidade
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdModalidade).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Modalidade> GetListIdDescricao()
        {
            List<Modalidade> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Modalidade
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Modalidade()
                        {
                            IdModalidade = y.IdModalidade,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdModalidade).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Modalidade Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Modalidade.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Modalidade Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Modalidade.Update(Entity);
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
