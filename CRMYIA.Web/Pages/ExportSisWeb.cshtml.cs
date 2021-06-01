using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.ExportacaoSisWeb;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class ExportSisWebModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public List<Proposta> ListEntity { get; set; }

        [BindProperty]
        public string DataPeriodo { get; set; }
        #endregion

        #region Construtores
        public ExportSisWebModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            DataPeriodo = DateTime.Now.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
            DateTime DataInicio = (DataPeriodo.Split('-')[0].Trim() + " 00:00:00").ExtractDateTime();
            DateTime DataFim = (DataPeriodo.Split('-')[1].Trim() + " 23:59:59").ExtractDateTime();
            ListEntity = PropostaModel.GetList(GetIdUsuario(), DataInicio, DataFim);
            HttpContext.Session.SetObjectAsJson("ListPropostaExportarSisWeb", ListEntity);
            return Page();
        }

        public IActionResult OnPost()
        {
            //DataPeriodo = DateTime.Now.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
            DateTime DataInicio = (DataPeriodo.Split('-')[0].Trim() + " 00:00:00").ExtractDateTime();
            DateTime DataFim = (DataPeriodo.Split('-')[1].Trim() + " 23:59:59").ExtractDateTime();
            ListEntity = PropostaModel.GetList(GetIdUsuario(), DataInicio, DataFim);
            HttpContext.Session.SetObjectAsJson("ListPropostaExportarSisWeb", ListEntity);
            return Page();
        }

        public async Task<IActionResult> OnGetDownloadAsync()
        {
            if (HttpContext.Session.GetObjectFromJson<List<Proposta>>("ListPropostaExportarSisWeb") != null)
            {
                ListEntity = HttpContext.Session.GetObjectFromJson<List<Proposta>>("ListPropostaExportarSisWeb");

                string CaminhoArquivo = ExportacaoSisWebModel.GerarExportacaoSisWeb(ListEntity);

                var memory = new MemoryStream();
                using (var stream = new FileStream(CaminhoArquivo, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, Util.GetContentType(CaminhoArquivo), Path.GetFileName(CaminhoArquivo));
            }
            else
                return null;
        }
        #endregion

        public long GetIdUsuario()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> Claims = identity.Claims;
            foreach (var t in Claims)
            {
                if (t.Type.Equals("IdUsuarioSlave"))
                    IdUsuario = t.Value.ExtractLong();
            }
            return IdUsuario;
        }
    }
}
