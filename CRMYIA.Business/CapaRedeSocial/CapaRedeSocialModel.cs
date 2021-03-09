using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CRMYIA.Business
{
    public class CapaRedeSocialModel
    {
        public static List<CapaRedeSocialViewModel> GetList()
        {
            List<CapaRedeSocialViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.CapaRedeSocial
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .Select(x => new CapaRedeSocialViewModel
                        {
                            IdUsuario = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdUsuario.ToString()).ToString()),
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeArquivo = x.NomeArquivo,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo
                        })
                        .OrderByDescending(o => o.IdUsuario).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
    }
}
