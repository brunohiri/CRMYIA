using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class MaterialDivulgacaoModel : PageModel
    {
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        #region Construtores
        public MaterialDivulgacaoModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion
        [BindProperty]
        public List<CampanhaArquivo> ListCampanhaArquivo { get; set; }
        public List<Campanha> ListCampanha { get; set; }
        public void OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                ListsCampanhaId();
            else
                ListsCampanhaId(Id);
        }

        private void ListsCampanhaId(string Id = null)
        {
            ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
        }

        private void ListsCampanhaId()
        {
            ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            ListCampanha = CampanhaModel.GetListOrderById();
        }
    }
}
