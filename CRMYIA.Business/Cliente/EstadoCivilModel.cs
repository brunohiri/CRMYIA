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
    public class EstadoCivilModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static EstadoCivil Get(long IdEstadoCivil)
        {
            EstadoCivil Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.EstadoCivil
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdEstadoCivil == IdEstadoCivil)
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

        public static EstadoCivil GetByDescricao(string Descricao)
        {
            EstadoCivil Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.EstadoCivil
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.Descricao.ToUpper().Contains(Descricao.RemoverAcentuacao().ToUpper()))
                        .AsNoTracking()
                        .FirstOrDefault();

                    //Se não encontrar, retorna OUTROS
                    if (Entity == null)
                        Entity = context.EstadoCivil.Where(x => x.IdEstadoCivil == 5).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<EstadoCivil> GetList()
        {
            List<EstadoCivil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.EstadoCivil
                        .Include(y => y.Cliente)
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

        public static List<EstadoCivil> GetListIdDescricao()
        {
            List<EstadoCivil> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.EstadoCivil
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new EstadoCivil()
                        {
                            IdEstadoCivil = y.IdEstadoCivil,
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

        public static void Add(EstadoCivil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.EstadoCivil.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(EstadoCivil Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.EstadoCivil.Update(Entity);
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
