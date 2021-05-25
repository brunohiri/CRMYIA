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
    public class GeneroModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Genero Get(long IdGenero)
        {
            Genero Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Genero
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdGenero == IdGenero)
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

        public static Genero GetByDescricao(string Descricao)
        {
            Genero Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Genero
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.Descricao.ToUpper().Contains(Descricao.RemoverAcentuacao().ToUpper()))
                        .AsNoTracking()
                        .FirstOrDefault();

                    //Se não encontrar, retorna OUTRO
                    if (Entity == null)
                        Entity = context.Genero.Where(x => x.IdGenero == 3).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Genero> GetList()
        {
            List<Genero> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Genero
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

        public static List<Genero> GetListIdDescricao()
        {
            List<Genero> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Genero
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Genero()
                        {
                            IdGenero = y.IdGenero,
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

        public static void Add(Genero Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Genero.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Genero Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Genero.Update(Entity);
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
