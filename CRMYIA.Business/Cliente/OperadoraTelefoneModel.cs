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
    public class OperadoraTelefoneModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static OperadoraTelefone Get(long IdOperadoraTelefone)
        {
            OperadoraTelefone Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.OperadoraTelefone
                        .AsNoTracking()
                        .Where(x => x.IdOperadoraTelefone == IdOperadoraTelefone)
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


        public static OperadoraTelefone GetByDescricao(string Descricao)
        {
            OperadoraTelefone Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.OperadoraTelefone
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.Descricao.ToUpper().Contains(Descricao.RemoverAcentuacao().ToUpper()))
                        .AsNoTracking()
                        .FirstOrDefault();

                    //Se não encontrar, retorna OUTRO
                    if (Entity == null)
                        Entity = context.OperadoraTelefone.Where(x => x.IdOperadoraTelefone == 5).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<OperadoraTelefone> GetList()
        {
            List<OperadoraTelefone> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.OperadoraTelefone
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

        public static List<OperadoraTelefone> GetListIdDescricao()
        {
            List<OperadoraTelefone> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.OperadoraTelefone
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .Select(y => new OperadoraTelefone()
                        {
                            IdOperadoraTelefone = y.IdOperadoraTelefone,
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

        public static void Add(OperadoraTelefone Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.OperadoraTelefone.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(OperadoraTelefone Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.OperadoraTelefone.Update(Entity);
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
