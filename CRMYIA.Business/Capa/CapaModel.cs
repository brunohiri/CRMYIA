using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CRMYIA.Business
{
    public class CapaModel
    {

        public static Capa Get(long IdCapa)
        {
            Capa Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Capa
                        .Where(x => x.IdCapa == IdCapa)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<CapaViewModel> GetList()
        {
            List<CapaViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CapaRedeSocial
                        .Include(x => x.IdRedeSocialNavigation)
                        .Include(x => x.IdCapaNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdCapaNavigation.Ativo)
                        .Select(x => new CapaViewModel() {
                            Id = x.IdCapaNavigation.IdCapa,
                            IdCapa = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdCapa.ToString()).ToString()),
                            Titulo = x.IdCapaNavigation.Titulo,
                            CaminhoArquivo = x.IdCapaNavigation.CaminhoArquivo,
                            NomeArquivo = x.IdCapaNavigation.NomeArquivo,
                            Width = x.IdCapaNavigation.Width.ToString(),
                            Heighgt = x.IdCapaNavigation.Height.ToString(),
                            DataCadastro = x.IdCapaNavigation.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.IdCapaNavigation.Ativo,
                            IdCapaNavigation = x.IdCapaNavigation,
                            IdRedeSocialNavigation = x.IdRedeSocialNavigation
                        })
                        .OrderBy(o => o.Titulo)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Update(Capa Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Capa.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Add(Capa Entity)
        {

            bool retorno = false;
            using (YiaContext context = new YiaContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Capa.Add(Entity);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        retorno = true;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return retorno;
        }

        public static Capa GetLastId()
        {
            Capa Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Capa
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

    }

}
