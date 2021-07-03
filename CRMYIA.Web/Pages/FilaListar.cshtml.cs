using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.ExportacaoSisWeb;
using CRMYIA.Business.Util;
using CRMYIA.Business.YNDICA;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class FilaListarModel : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoProcessar { get; set; }

        [BindProperty]
        public byte IdFornecedor { get; set; }

        [BindProperty]
        public List<Fornecedor> ListFornecedor { get; set; }

        [BindProperty]
        public byte IdLayout { get; set; }

        [BindProperty]
        public List<Layout> ListLayout { get; set; }

        [BindProperty]
        public List<Fila> ListEntity { get; set; }

        #endregion

        #region Construtores
        public FilaListarModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnGetDownloadAsync(string filename)
        {
            try
            {
                Fila Entity = null;
                long IdFila = 0;
                var isNumeric = long.TryParse(filename, out IdFila);
                if (isNumeric)
                    IdFila = long.Parse(filename);
                else
                    IdFila = long.Parse(Criptography.Decrypt(filename));

                Entity = FilaModel.Get(IdFila);

                var path = Path.Combine(_configuration["YndicaProcessado"], Entity.NomeArquivoSaida);

                string CaminhoArquivoZip = Path.Combine(_configuration["YndicaProcessado"], Entity.NomeArquivoSaida.Replace(".csv", ".zip"));
                string[] CaminhoArquivos = { path };
                Util.CompactarArquivo(CaminhoArquivoZip, CaminhoArquivos);

                var memory = new MemoryStream();
                using (var stream = new FileStream(CaminhoArquivoZip, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, Util.GetContentType(CaminhoArquivoZip), Path.GetFileName(CaminhoArquivoZip));

            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, ex.Message);
            }
            return Page();
        }

        #endregion

        #region Outros Métodos

        public void CarregarLists()
        {
            ListFornecedor = FornecedorModel.GetListIdDescricao();
            ListLayout = LayoutModel.GetListIdDescricao();
            ListEntity = FilaModel.GetList();
        }

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

        #endregion
    }
}
