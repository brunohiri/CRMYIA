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

        public static List<BannerOperadoraViewModel> GetList()
        {
            List<BannerOperadoraViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Banner
                        .Include(x => x.IdInformacaoNavigation)
                        .Include(x => x.IdCampanhaNavigation)
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

        public static List<BannerOperadoraViewModel> GetAllBanner(List<long> IdBanner, byte IdGrupoCorretor)
        {
            List<BannerOperadoraViewModel> ListEntity = new List<BannerOperadoraViewModel>();
            List<BannerOperadoraViewModel> ListBannerOperadora = null;
            List<Visita> ListVisita = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListVisita = context.Visita
                    .Include(x => x.IdCalendarioSazonalNavigation)
                    .OrderBy(x => x.DataAgendamento)
                    .AsNoTracking()
                    .ToList();

                    ListBannerOperadora = context.Banner
                        .Include(x => x.IdInformacaoNavigation)
                        .Include(x => x.IdCampanhaNavigation)
                            .ThenInclude(x => x.GrupoCorretorCampanha)
                        .Where(x => x.Ativo && x.IdCampanhaNavigation.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == IdGrupoCorretor).Count() > 0)
                        .Select(x => new BannerOperadoraViewModel()
                        {
                            IdBanner = x.IdBanner.ToString(),
                            IdCampanha = x.IdCampanhaNavigation.IdCampanha.ToString(),
                            Titulo = x.IdInformacaoNavigation.Titulo,
                            Descricao = x.IdInformacaoNavigation.Descricao,
                            NomeCampanha = x.IdCampanhaNavigation.Descricao,
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            Width = x.Width,
                            Height = x.Height,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo,
                            IdInformacaoNavigation = x.IdInformacaoNavigation
                        })
                        .ToList();
                    foreach (BannerOperadoraViewModel Item in ListBannerOperadora)
                    {
                        if (Item.IdCalendario == null)
                        {
                            ListEntity.Add(Item);
                        }
                        foreach (var ItemVisita in ListVisita)
                        {
                            if (Item.IdCalendario != null && ItemVisita.IdCalendarioSazonalNavigation != null)
                            {
                                if (ItemVisita.IdCalendarioSazonalNavigation.IdCalendario == Item.IdCalendario &&
                                  new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) >= new DateTime(Convert.ToInt32(ItemVisita.DataInicio?.Year), Convert.ToInt32(ItemVisita.DataInicio?.Month), Convert.ToInt32(ItemVisita.DataInicio?.Day)) &&
                                  new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) <= new DateTime(Convert.ToInt32(ItemVisita.DataFim?.Year), Convert.ToInt32(ItemVisita.DataFim?.Month), Convert.ToInt32(ItemVisita.DataFim?.Day)))
                                {
                                    ListEntity.Add(Item);
                                }
                            }
                        }
                    }
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
