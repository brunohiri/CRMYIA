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

        public static List<AssinaturaCartaoViewModel> GetListaAssinatura(long IdCampanha, byte IdGrupoCorretor)
        {
            //List<AssinaturaCartaoViewModel> ListEntity = new List<CapaViewModel>();
            List<AssinaturaCartaoViewModel> ListAssinaturaCartao = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListAssinaturaCartao = context.AssinaturaCartao
                    .Include(x => x.IdCampanhaNavigation)
                        .ThenInclude(x => x.GrupoCorretorCampanha)
                    .Where(x => x.IdCampanha == IdCampanha && x.IdCampanhaNavigation.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == IdGrupoCorretor).Count() > 0)
                    .Select(x => new AssinaturaCartaoViewModel()
                        {
                            IdAssinaturaCartao = x.IdAssinaturaCartao.ToString(),
                            IdCampanha = x.IdCampanhaNavigation.IdCampanha.ToString(),
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
            }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListAssinaturaCartao;
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
