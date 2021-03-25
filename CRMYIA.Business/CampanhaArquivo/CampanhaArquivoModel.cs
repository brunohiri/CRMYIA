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

        public static List<CampanhaArquivo> GetListaCampanhaArquivo()
        {
            List<CampanhaArquivo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .Include(x => x.IdInformacaoNavigation)
                        .OrderBy(o => o.IdCampanha)
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<CampanhaArquivo> GetListaCampanhaArquivo(long Id)
        {
            List<CampanhaArquivo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CampanhaArquivo
                        .Include(x => x.IdCampanhaNavigation)
                        .Include(x => x.IdInformacaoNavigation)
                        .Where(x => x.IdCampanhaNavigation.IdCampanha == Id)
                        .OrderBy(o => o.IdCampanhaNavigation.Descricao)
                        .AsNoTracking()
                        .ToList();
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
