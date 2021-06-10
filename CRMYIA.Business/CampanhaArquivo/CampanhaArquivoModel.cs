using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class CampanhaArquivoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static CampanhaArquivo Get(long IdArquivo)
        {
            CampanhaArquivo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CampanhaArquivo
                        .Where(x => x.IdCampanhaArquivo == IdArquivo)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static bool GetCampanhaId(long IdCampanha)
        {
            CampanhaArquivo Entity = null;
            bool retorno;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CampanhaArquivo
                        .Where(x => x.IdCampanha == IdCampanha)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }

            retorno = Entity == null? false: true ;
            //return retorno;
            return retorno;
        }

        public static CampanhaArquivo GetLastId()
        {
            CampanhaArquivo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.CampanhaArquivo
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

        //public static List<Campanha> GetListaCampanhaArquivo()
        //{
        //    List<Campanha> ListEntity = null;

        //    //List<CampanhaArquivo> List = null;
        //    //List<Video> List = null;
        //    //List<> List = null;
        //    //List<> List = null;
        //    //List<> List = null;

        //    try
        //    {
        //        using (YiaContext context = new YiaContext())
        //        {
        //            ListEntity = context.Campanha.Where(x => x.Ativo)
        //                .Include(x => x.CampanhaArquivo)
        //                .Include(x=>x.Video)
        //                .Include(x=>x.RedeSocial)
        //                .Include(x=>x.AssinaturaCartao)
        //                .Include(x=>x.Banner)
        //                .AsNoTracking()
        //                .ToList();
        //        }
                    
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return ListEntity;
        //}

        //public static List<CampanhaArquivo> GetListaCampanhaArquivo(long Id)
        //{
        //    List<CampanhaArquivo> ListEntity = null;
        //    try
        //    {
        //        using (YiaContext context = new YiaContext())
        //        {
        //            ListEntity = context.CampanhaArquivo
        //                .Include(x => x.IdCampanhaNavigation)
        //                .Include(x => x.IdInformacaoNavigation)
        //                .Where(x => x.IdCampanha == Id)
        //                .OrderBy(o => o.IdCampanhaNavigation.Descricao)
        //                .AsNoTracking()
        //                .ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return ListEntity;
        //}

        public static List<CampanhaArquivo> GetListaCampanhaArquivo(byte IdGrupoCorretor)
        {
            List<CampanhaArquivo> ListEntity = new List<CampanhaArquivo>();
            List<CampanhaArquivo> ListCampanhaArquivo = new List<CampanhaArquivo>();
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

                    ListCampanhaArquivo = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .Include(x => x.IdInformacaoNavigation)
                        .Include(x => x.IdCalendarioNavigation)
                            .ThenInclude(x => x.CalendarioSazonal)
                            .ThenInclude(x => x.Visita)
                        .Where(x => x.IdCampanhaNavigation.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == IdGrupoCorretor).Count() > 0)
                        .AsNoTracking()
                        .ToList();
                        foreach (CampanhaArquivo ItemCampanhaArquivo in ListCampanhaArquivo)
                        { 
                            if(ItemCampanhaArquivo.IdCalendario == null)
                            {
                                ListEntity.Add(ItemCampanhaArquivo);
                            }
                            foreach (Visita ItemVisita in ListVisita)
                            {
                                if (ItemCampanhaArquivo.IdCalendario != null && ItemVisita.IdCalendarioSazonalNavigation != null)
                                {
                                    if (ItemVisita.IdCalendarioSazonalNavigation.IdCalendario == ItemCampanhaArquivo.IdCalendario &&
                                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) >= new DateTime(Convert.ToInt32(ItemVisita.DataInicio?.Year), Convert.ToInt32(ItemVisita.DataInicio?.Month), Convert.ToInt32(ItemVisita.DataInicio?.Day)) &&
                                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) <= new DateTime(Convert.ToInt32(ItemVisita.DataFim?.Year), Convert.ToInt32(ItemVisita.DataFim?.Month), Convert.ToInt32(ItemVisita.DataFim?.Day)))
                                    {
                                        ListEntity.Add(ItemCampanhaArquivo);
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

        public static List<MaterialDivulgacaoViewModel> GetList()
        {
            List<MaterialDivulgacaoViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .Include(x => x.IdInformacaoNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .Select(x => new MaterialDivulgacaoViewModel
                        {
                            //Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong()
                            //System.Web.HttpUtility.UrlEncode(Criptography.Encrypt(Item.IdCampanhaArquivo.ToString()))
                            IdCampanhaArquivo = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdCampanhaArquivo.ToString()).ToString()),
                            IdCampanha = x.IdCampanha.ToString(),
                            Descricao = x.IdInformacaoNavigation.Descricao,
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo,
                            IdCampanhaNavigation = x.IdCampanhaNavigation
                        })
                        .OrderByDescending(o => o.IdCampanha).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(CampanhaArquivo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CampanhaArquivo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(CampanhaArquivo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.CampanhaArquivo.Update(Entity);
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
