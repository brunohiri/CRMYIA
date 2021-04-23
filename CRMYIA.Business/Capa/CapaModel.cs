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

        public static List<CapaViewModel> GetListaCapa(long IdCampanha, byte IdGrupoCorretor)
        {
            List<CapaViewModel> ListEntity = new List<CapaViewModel>();
            List<Capa> ListCapa = null; 
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListCapa = context.Capa
                        .Include(x => x.CapaRedeSocial)
                            .ThenInclude(x => x.IdCampanhaNavigation)
                                .ThenInclude(x => x.GrupoCorretorCampanha)
                       .Where(x => x.CapaRedeSocial.Where(x => x.IdCampanha == IdCampanha).Count() > 0 && x.CapaRedeSocial.Where(x => x.IdCampanhaNavigation.GrupoCorretorCampanha.Any(x => x.IdGrupoCorretor == IdGrupoCorretor)).Count() > 0 )
                       .AsNoTracking()
                       .ToList();

                    foreach (var Item in ListCapa)
                    {
                        ListEntity.Add(new CapaViewModel()
                        {
                            Id = Item.CapaRedeSocial.Select(x => x.IdCapaNavigation.IdCapa).First(),
                            IdCapa = Item.IdCapa.ToString(),
                            IdCampanha = Item.CapaRedeSocial.Select(x => x.IdCampanhaNavigation.IdCampanha).First().ToString(),
                            Titulo = Item.Titulo,
                            CaminhoArquivo = Item.CaminhoArquivo,
                            NomeArquivo = Item.NomeArquivo,
                            Heighgt = Item.Height.ToString(),
                            Width = Item.Width.ToString(),
                            DataCadastro = Item.DataCadastro.ToString(),
                            Ativo = Item.Ativo,
                            IdCapaNavigation = Item.CapaRedeSocial.Select(x => x.IdCapaNavigation).First(),
                            IdRedeSocialNavigation = Item.CapaRedeSocial.Select(x => x.IdRedeSocialNavigation).First(),
                        });
                    }
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
