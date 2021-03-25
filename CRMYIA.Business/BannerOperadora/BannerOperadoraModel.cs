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
    public class BannerOperadoraModel
    {
        public static Banner Get(long IdBanner)
        {
            Banner Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Banner
                        .Where(x => x.IdBanner == IdBanner)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<BannerOperadora> GetAllBannerOperadora(long IdOperadora)
        {
            List<BannerOperadora> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.BannerOperadora
                        .Where(x => x.IdOperadora == IdOperadora)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<BannerOperadoraViewModel> GetAllBanner(List<long> IdBanner)
        {
            List<BannerOperadoraViewModel> Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.BannerOperadora
                        .Include(x => x.IdOperadoraNavigation)
                        .Include(x => x.IdBannerNavigation)
                            .ThenInclude(x => x.IdInformacaoNavigation)
                        .Where(x => IdBanner.Contains(x.IdBannerNavigation.IdBanner) && x.IdBannerNavigation.Ativo)
                        .Select(x => new BannerOperadoraViewModel() {
                            IdOperadora = x.IdOperadoraNavigation.IdOperadora.ToString(),
                            IdBanner = x.IdBannerNavigation.IdBanner.ToString(),
                            Titulo = x.IdOperadoraNavigation.Descricao,
                            Descricao = x.IdBannerNavigation.IdInformacaoNavigation.Descricao,
                            CaminhoArquivo = x.IdBannerNavigation.CaminhoArquivo,
                            NomeArquivo = x.IdBannerNavigation.NomeArquivo,
                            Width = x.IdBannerNavigation.Width,
                            Height = x.IdBannerNavigation.Height,
                            DataCadastro = x.IdBannerNavigation.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.IdBannerNavigation.Ativo,
                            IdOperadoraNavigation = x.IdOperadoraNavigation,
                        })
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static BannerOperadora Get(string IdBanner)
        {
            BannerOperadora Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.BannerOperadora
                        .Where(x => x.IdBanner == IdBanner.ExtractLong())
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<BannerOperadoraViewModel> GetList()
        {
            List<BannerOperadoraViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Banner
                        .Include(x => x.IdInformacaoNavigation)
                        .Include(x => x.BannerOperadora)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .Select(x => new BannerOperadoraViewModel()
                        {
                            IdBanner = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdBanner.ToString()).ToString()),
                            Descricao = x.IdInformacaoNavigation.Descricao,
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            Width = x.Width,
                            Height = x.Height,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo
                        })
                        .OrderBy(o => o.Descricao)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static bool AddBool(Banner Entity)
        {
            bool retorno = false;
            using (YiaContext context = new YiaContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Banner.Add(Entity);
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

        public static void Add(BannerOperadora Entity)
        {
            using (YiaContext context = new YiaContext())
            {
                try
                {
                    context.BannerOperadora.Add(Entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public static void Update(Banner Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Banner.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void Update(BannerOperadora Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.BannerOperadora.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static Banner GetLastId()
        {
            Banner Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Banner
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
