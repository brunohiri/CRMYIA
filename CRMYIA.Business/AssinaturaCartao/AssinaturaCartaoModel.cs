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
    public class AssinaturaCartaoModel
    {
        public static AssinaturaCartao Get(long IdAssinaturaCartao)
        {
            AssinaturaCartao Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.AssinaturaCartao
                        .Where(x => x.IdAssinaturaCartao == IdAssinaturaCartao)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<AssinaturaCartaoViewModel> GetList()
        {
            List<AssinaturaCartaoViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.AssinaturaCartao
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .Select(x => new AssinaturaCartaoViewModel()
                        {
                            IdAssinaturaCartao = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdAssinaturaCartao.ToString()).ToString()),
                            Titulo = x.Titulo,
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            Width = x.Width,
                            Height = x.Height,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo,
                            IdUsuarioNavigation = x.IdUsuarioNavigation
                        })
                        .OrderBy(o => o.NomeArquivo)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<AssinaturaCartaoViewModel> GetListaAssinatura(/*long IdCampanha,*/ byte IdGrupoCorretor)
        {
            List<AssinaturaCartaoViewModel> ListEntity = new List<AssinaturaCartaoViewModel>();
            List<AssinaturaCartaoViewModel> ListAssinaturaCartao = null;
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

                    ListAssinaturaCartao = context.AssinaturaCartao
                    .Include(x => x.IdCampanhaNavigation)
                        .ThenInclude(x => x.GrupoCorretorCampanha)
                    .Where(x => x.IdCampanhaNavigation.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == IdGrupoCorretor).Count() > 0)
                    .Select(x => new AssinaturaCartaoViewModel()
                        {
                            IdAssinaturaCartao = x.IdAssinaturaCartao.ToString(),
                            IdCampanha = x.IdCampanhaNavigation.IdCampanha.ToString(),
                            IdCalendario = x.IdCalendario,
                            Titulo = x.Titulo,
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            Width = x.Width,
                            Height = x.Height,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo,
                            IdUsuarioNavigation = x.IdUsuarioNavigation
                        })
                    .AsNoTracking()
                    .ToList();

                    foreach (AssinaturaCartaoViewModel Item in ListAssinaturaCartao)
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
            catch (Exception ex)
            {
                throw;
            }
            return ListEntity;
        }
        public static void Add(AssinaturaCartao Entity)
        {

            using (YiaContext context = new YiaContext())
            {
                try
                {
                    context.AssinaturaCartao.Add(Entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public static void Update(AssinaturaCartao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.AssinaturaCartao.Update(Entity);
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
