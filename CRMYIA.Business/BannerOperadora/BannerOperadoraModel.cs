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

        public static List<BannerOperadoraViewModel> GetList()
        {
            List<BannerOperadoraViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Banner
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .Select(x => new BannerOperadoraViewModel()
                        {
                            IdBanner = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdBanner.ToString()).ToString()),
                            Descricao = x.Descricao,
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
        public static bool Add(Banner Entity)
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

    }
}
