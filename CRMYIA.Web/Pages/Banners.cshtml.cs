using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
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
    public class BannersModel : PageModel
    {
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        #region Construtores
        public BannersModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        //[BindProperty]
        //public List<CampanhaArquivoViewModel> ListaCampanhaArquivo { get; set; }

        //[BindProperty]
        //public List<CampanhaArquivo> ListCampanhaArquivo { get; set; }
        //public List<Campanha> ListCampanha { get; set; }
        //public void OnGet(string Id = null)
        //{
        //    //PublishUrl = Title;
        //    if (Id.IsNullOrEmpty())
        //        ListsCampanhaId();
        //    else
        //        ListsCampanhaId(Id);

        //    OnGetListarCampanha();
        //}

        public IActionResult OnPostListarCampanha()
        {
            string Id = Request.Form["Id"].ToString();
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            Usuario EntityUsuario = null;
            EntityUsuario = UsuarioModel.Get(IdUsuario);

            bool status = false;
            int i;

            long IdCampanhaArquivo = 0;
            long? IdCampanha = 0;
            string Descricao = "";
            string Titulo = "";
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
            List<CampanhaArquivo> CampanhaArquivo = CampanhaArquivoModel.GetListaCampanhaArquivo(Criptography.Decrypt(Id).ExtractLong(), (byte)EntityUsuario.IdGrupoCorretor);
            List<Informacao> EntityInformacao = InformacaoModel.Get();
            UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);

            foreach (var ItemInformacao in EntityInformacao)
            {
                i = 0;
                foreach (var ItemCampanhaArquivo in CampanhaArquivo)
                {
                    if (ItemInformacao.IdInformacao == ItemCampanhaArquivo.IdInformacaoNavigation.IdInformacao)
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
                        Titulo = ItemCampanhaArquivo.IdInformacaoNavigation.Titulo;
                        Descricao = ItemCampanhaArquivo.IdInformacaoNavigation.Descricao;
                        CaminhoArquivo = ItemCampanhaArquivo.CaminhoArquivo;
                        RedesSociais = ItemCampanhaArquivo.RedesSociais;
                        TipoPostagem = ItemCampanhaArquivo.TipoPostagem;
                        DataCadastro = ItemCampanhaArquivo.DataCadastro;
                        Ativo = ItemCampanhaArquivo.Ativo;
                        IdCampanhaNavigation = ItemCampanhaArquivo.IdCampanhaNavigation;
                        IdInformacaoNavigation = ItemCampanhaArquivo.IdInformacaoNavigation;
                        i++;
                    }

                }

                if (i > 0)
                {
                    AuxCampanhaArquivo.Add(new CampanhaArquivoViewModel()
                    {
                        IdCampanhaArquivo = IdCampanhaArquivo,
                        IdCampanha = IdCampanha,
                        Titulo = Titulo,
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
                }
                NomeArquivo = "";
                Width = "";
                Height = "";

            }
            if (AuxCampanhaArquivo != null)
            {
                status = true;
            }
            return new JsonResult(new { status = status, campanhaArquivo = AuxCampanhaArquivo, campanha = EntityInformacao, usuarioEntity = UsuarioEntity });
        }

        public IActionResult OnPostContadorDownload(Campanha fromData)
        {
            bool status = false;
            try
            {
                if(fromData.IdCampanha > 0 && fromData.IdCampanha.ToString() != null)
                {
                   Campanha EntityCampanha = CampanhaModel.Get(fromData.IdCampanha);
                    EntityCampanha.QuantidadeDownload = (EntityCampanha.QuantidadeDownload + 1);
                   CampanhaModel.Update(EntityCampanha);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult(new
            {
                status = status
            });
        }

    }
}
