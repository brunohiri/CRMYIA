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
    public class EnriquecimentoModel : PageModel
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
        public List<ListaClienteViewModel> ListEntity { get; set; }

        #endregion

        #region Construtores
        public EnriquecimentoModel(IConfiguration configuration, IHostingEnvironment environment)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Campos Incorretos!");
            }
            else
            if (IdFornecedor == 0)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Selecione um Fornecedor Válido!");
            }
            else
             if (IdLayout == 0)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Selecione um Layout Válido!");
            }
            else
            {
                if (Mensagem == null)
                {
                    string NomeArquivoOriginal = NomeArquivoProcessar.FileName;
                    string NomeArquivo = string.Empty;

                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var file = Path.Combine(_environment.WebRootPath, _configuration["YndicaProcessar"], NomeArquivo);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await NomeArquivoProcessar.CopyToAsync(fileStream);
                    }

                    FilaModel.Add(new Fila()
                    {
                        IdStatusFila = (byte?)(EnumeradorModel.StatusFila.Aguardando),
                        IdUsuario = GetIdUsuario(),
                        IdFornecedor = IdFornecedor,
                        IdLayout = IdLayout,
                        NomeJob = NomeArquivo,
                        CaminhoArquivoEntrada = Path.Combine(_environment.WebRootPath, _configuration["YndicaProcessar"]),
                        NomeArquivoEntrada = NomeArquivo,
                        CaminhoArquivoSaida = Path.Combine(_environment.WebRootPath, _configuration["YndicaProcessado"]),
                        NomeArquivoSaida = NomeArquivo.Replace("_0", "_0_Processado"),
                        QtdEntrada = 0,
                        QtdProcessado = 0,
                        QtdSaida = 0,
                        DataEntrada = DateTime.Now
                    });

                    Fila EntityFila = FilaModel.GetLastId();

                    int QtdRegistros = ImportarArquivo(file, EntityFila.IdFila);

                    EntityFila.QtdEntrada = QtdRegistros;
                    FilaModel.Update(EntityFila);

                    Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Sucesso, string.Format("O arquivo com {0:#,0} registros foi importado com sucesso!", Convert.ToInt64(QtdRegistros)));
                }
            }

            CarregarLists();
            return Page();
        }

        #endregion

        #region Outros Métodos

        public int ImportarArquivo(string ArquivoCompleto, long IdFila)
        {
            int QtdRegistros = 0;
            if (System.IO.File.Exists(ArquivoCompleto))
            {
                string[] Linhas = System.IO.File.ReadAllLines(ArquivoCompleto);

                foreach (var Item in Linhas)
                {
                    if (QtdRegistros > 0)
                    {
                        FilaItemModel.Add(new FilaItem()
                        {
                            IdFila = IdFila,
                            Documento = Item,
                            DataCadastro = DateTime.Now,
                            Processado = false
                        });
                    }
                    QtdRegistros++;
                }
            }
            return QtdRegistros;
        }

        public void CarregarLists()
        {
            ListFornecedor = FornecedorModel.GetListIdDescricao();
            ListLayout = LayoutModel.GetListIdDescricao();
            ListEntity = ClienteModel.GetList(null, null, null, null, null, null);
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
