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
    public class ClassificacaoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Classificacao Get(long IdClassificacao)
        {
            Classificacao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Classificacao
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdClassificacao == IdClassificacao)
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

        public static List<Classificacao> GetList()
        {
            List<Classificacao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Classificacao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderBy(o => o.IdClassificacao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Classificacao> GetListIdDescricao()
        {
            List<Classificacao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Classificacao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new Classificacao()
                        {
                            IdClassificacao = y.IdClassificacao,
                            Descricao = y.Descricao
                        }).OrderBy(o => o.IdClassificacao).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Classificacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Classificacao.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Classificacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Classificacao.Update(Entity);
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
