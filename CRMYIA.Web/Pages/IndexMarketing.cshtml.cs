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
    public class IndexMarketingModel : PageModel
    {
        private readonly ILogger<IndexSupervisorModel> _logger;

        public IndexMarketingModel(ILogger<IndexSupervisorModel> logger)
        {
            _logger = logger;
        }

        public List<Campanha> ListaCampanhaArquivo { get; set; }
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

            List<Campanha> AuxCampanhaArquivo = new List<Campanha>();
            List<Campanha> Campanha = CampanhaModel.GetListaCampanha();
            List<Campanha> GrupoCorretorCampanhaEntity = null;
            UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
            Usuario EntityUsuario = UsuarioModel.Get(IdUsuario);
            foreach (var ItemCampanha in Campanha)
            {
                GrupoCorretorCampanhaEntity = GrupoCorretorCampanhaModel.GetListLinks(ItemCampanha.IdCampanha);

                foreach (var ItemGrupoCorretorCampanha in GrupoCorretorCampanhaEntity)
                {
                    if (ItemGrupoCorretorCampanha.IdCampanha == ItemCampanha.IdCampanha && ItemGrupoCorretorCampanha.GrupoCorretorCampanha.Where(x => x.IdGrupoCorretor == EntityUsuario.IdGrupoCorretor).Count() > 0)
                    {
                        AuxCampanhaArquivo.Add(new Campanha()
                        {
                            IdCampanha = ItemCampanha.IdCampanha,
                            IdUsuario = ItemCampanha.IdUsuario,
                            Descricao = ItemCampanha.Descricao,
                            CaminhoArquivo = ItemCampanha.CaminhoArquivo,
                            NomeArquivo = ItemCampanha.NomeArquivo,
                            DataCadastro = ItemCampanha.DataCadastro,
                            Ativo = ItemCampanha.Ativo,
                            IdUsuarioNavigation = ItemGrupoCorretorCampanha.IdUsuarioNavigation,
                            AssinaturaCartao = ItemGrupoCorretorCampanha.AssinaturaCartao,
                            Banner = ItemGrupoCorretorCampanha.Banner,
                            CapaRedeSocial = ItemGrupoCorretorCampanha.CapaRedeSocial,
                            CampanhaArquivo = ItemGrupoCorretorCampanha.CampanhaArquivo,
                            GrupoCorretorCampanha = ItemGrupoCorretorCampanha.GrupoCorretorCampanha,
                            Video = ItemGrupoCorretorCampanha.Video
                        });
                    }
                }

            }
            if (AuxCampanhaArquivo != null && Campanha != null)
            {
                status = true;
                ListaCampanhaArquivo = AuxCampanhaArquivo;
            }
        }
    }
}
