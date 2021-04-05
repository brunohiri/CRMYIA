using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class IndexSupervisorModel : PageModel
    {
        private readonly ILogger<IndexSupervisorModel> _logger;

        public IndexSupervisorModel(ILogger<IndexSupervisorModel> logger)
        {
            _logger = logger;
        }

        public List<CampanhaArquivoViewModel> ListaCampanhaArquivo { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<List<Modulo>>("MODULO") == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToPage("Login");
            }
            ListarCampanha();
            return Page();
        }

        public void ListarCampanha()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            int i;

            long IdCampanhaArquivo = 0;
            long? IdCampanha = 0;
            string Descricao = "";
            string CaminhoArquivo = "";
            string NomeArquivo = "";
            string Width = "";
            string Height = "";
            string RedesSociais = "";
            string TipoPostagem = "";
            DateTime DataCadastro = DateTime.MinValue;
            bool Ativo = false;
            Campanha IdCampanhaNavigation = new Campanha();
            Informacao IdInformacaoNavigation = new Informacao();

            List<CampanhaArquivoViewModel> AuxCampanhaArquivo = new List<CampanhaArquivoViewModel>();
            List<CampanhaArquivo> CampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            List<Campanha> Campanha = CampanhaModel.GetListOrderById();
            UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);

            foreach (var ItemCampanha in Campanha)
            {
                i = 0;
                foreach (var ItemCampanhaArquivo in CampanhaArquivo)
                {
                    if (ItemCampanha.IdCampanha == ItemCampanhaArquivo.IdCampanha && ItemCampanhaArquivo.IdInformacao == ItemCampanhaArquivo.IdInformacaoNavigation.IdInformacao)
                    {

                        if (i + 1 < CampanhaArquivo.Count)
                        {
                            NomeArquivo += ItemCampanhaArquivo.NomeArquivo + "|";
                            Width += ItemCampanhaArquivo.Width + "|";
                            Height += ItemCampanhaArquivo.Height + "|";
                        }
                        else
                        {
                            NomeArquivo += ItemCampanhaArquivo.NomeArquivo;
                            Width += ItemCampanhaArquivo.Width;
                            Height += ItemCampanhaArquivo.Height;

                        }
                        IdCampanhaArquivo = ItemCampanhaArquivo.IdCampanhaArquivo;
                        IdCampanha = ItemCampanhaArquivo.IdCampanha;
                        Descricao = ItemCampanhaArquivo.IdInformacaoNavigation.Descricao;
                        CaminhoArquivo = ItemCampanhaArquivo.CaminhoArquivo;
                        RedesSociais = ItemCampanhaArquivo.RedesSociais;
                        TipoPostagem = ItemCampanhaArquivo.TipoPostagem;
                        DataCadastro = ItemCampanhaArquivo.DataCadastro;
                        Ativo = ItemCampanhaArquivo.Ativo;
                        IdCampanhaNavigation = ItemCampanhaArquivo.IdCampanhaNavigation;
                        IdInformacaoNavigation = ItemCampanhaArquivo.IdInformacaoNavigation;
                    }
                    i++;
                }

                AuxCampanhaArquivo.Add(new CampanhaArquivoViewModel()
                {
                    IdCampanhaArquivo = IdCampanhaArquivo,
                    IdCampanha = IdCampanha,
                    Descricao = Descricao,
                    CaminhoArquivo = CaminhoArquivo,
                    NomeArquivo = NomeArquivo,
                    Width = Width,
                    Height = Height,
                    RedesSociais = RedesSociais,
                    TipoPostagem = TipoPostagem,
                    DataCadastro = DataCadastro,
                    Ativo = Ativo,
                    IdCampanhaNavigation = IdCampanhaNavigation,
                    IdInformacaoNavigation = IdInformacaoNavigation
                });
                NomeArquivo = "";
                Width = "";
                Height = "";

            }
            if (AuxCampanhaArquivo != null && Campanha != null)
            {
                status = true;
                ListaCampanhaArquivo = AuxCampanhaArquivo;
            }
            //return new JsonResult(new { status = status, campanhaArquivo = AuxCampanhaArquivo, campanha = Campanha, usuarioEntity = UsuarioEntity });
        }
    }
}
