using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class InformacaoModel
    {

        public static List<Informacao> Get()
        {
            List<Informacao> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Informacao
                        .Where(x => x.Ativo)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static Informacao Get(long IdInformacao)
        {
            Informacao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Informacao
                        .Where(x => x.IdInformacao == IdInformacao)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static void Add(Informacao Entity)
        {
            using (YiaContext context = new YiaContext())
            {
                try
                {
                    context.Informacao.Add(Entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static Informacao GetLastId()
        {
            Informacao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Informacao
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

        public static void Update(Informacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Informacao.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
