using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
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
        public List<Campanha> ListaCampanhaArquivo { get; set; }

        [BindProperty]
        public List<CampanhaArquivo> ListCampanhaArquivo { get; set; }
        public List<Campanha> ListCampanha { get; set; }
        public void OnGet(string Id = null)
        {
            //PublishUrl = Title;
            if (Id.IsNullOrEmpty())
            {
                ListsCampanhaId();
            }
            else
            {
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                Usuario EntityUsuario = null;
                EntityUsuario = UsuarioModel.Get(IdUsuario);
                ListsCampanhaId(Id, (byte)EntityUsuario.IdGrupoCorretor);
            }

            ListarCampanha();
        }

        public IActionResult OnGetListarCampanha()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            int i;

            long IdCampanhaArquivo = 0;
            long? IdCampanha = 0;
            string NomeCampanha = "";
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
           // List<CampanhaArquivo> CampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            List<Campanha> Campanha = CampanhaModel.GetListOrderById();
            //UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);

            //foreach (var ItemCampanha in Campanha)
            //{
            //    i = 0;
            //    foreach (var ItemCampanhaArquivo in CampanhaArquivo)
            //    {

            //        if (i + 1 < CampanhaArquivo.Count)
            //        {
            //            NomeArquivo += ItemCampanhaArquivo.NomeArquivo + "|";
            //            Width += ItemCampanhaArquivo.Width + "|";
            //            Height += ItemCampanhaArquivo.Height + "|";
            //        }
            //        else
            //        {
            //            NomeArquivo += ItemCampanhaArquivo.NomeArquivo;
            //            Width += ItemCampanhaArquivo.Width;
            //            Height += ItemCampanhaArquivo.Height;
                          
            //        }
            //        IdCampanhaArquivo = ItemCampanhaArquivo.IdCampanhaArquivo;
            //        IdCampanha = ItemCampanhaArquivo.IdCampanha;
            //        NomeCampanha = ItemCampanha.Descricao;
            //        Descricao = ItemCampanhaArquivo.IdInformacaoNavigation.Descricao;
            //        CaminhoArquivo = ItemCampanhaArquivo.CaminhoArquivo;
            //        RedesSociais = ItemCampanhaArquivo.RedesSociais;
            //        TipoPostagem = ItemCampanhaArquivo.TipoPostagem;
            //        DataCadastro = ItemCampanhaArquivo.DataCadastro;
            //        Ativo = ItemCampanhaArquivo.Ativo;
            //        IdCampanhaNavigation = ItemCampanhaArquivo.IdCampanhaNavigation;
            //        IdInformacaoNavigation = ItemCampanhaArquivo.IdInformacaoNavigation;
            //        i++;
            //    }

            //    AuxCampanhaArquivo.Add(new CampanhaArquivoViewModel()
            //    {
            //        IdCampanhaArquivo = IdCampanhaArquivo,
            //        IdCampanha = IdCampanha,
            //        NomeCampanha = NomeCampanha,
            //        Descricao = Descricao,
            //        CaminhoArquivo = CaminhoArquivo,
            //        NomeArquivo = NomeArquivo,
            //        Width = Width,
            //        Height = Height,
            //        RedesSociais = RedesSociais,
            //        TipoPostagem = TipoPostagem,
            //        DataCadastro = DataCadastro,
            //        Ativo = Ativo,
            //        IdCampanhaNavigation = IdCampanhaNavigation,
            //        IdInformacaoNavigation = IdInformacaoNavigation
            //    });
            //    NomeArquivo = "";
            //    Width = "";
            //    Height = "";

            //}  
            //if (AuxCampanhaArquivo != null && Campanha != null)
            //{
            //    status = true;
            //    ListaCampanhaArquivo = AuxCampanhaArquivo;
            //}
            return Page();
            //return new JsonResult(new { status = status, campanhaArquivo = AuxCampanhaArquivo, campanha = Campanha, usuarioEntity = UsuarioEntity });
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

        private void ListsCampanhaId(string Id, byte IdCorretor)
        {
            ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong(), IdCorretor);
        }

        private void ListsCampanhaId()
        {
            //ListCampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo();
            ListCampanha = CampanhaModel.GetListOrderById();
        }
    }
}
