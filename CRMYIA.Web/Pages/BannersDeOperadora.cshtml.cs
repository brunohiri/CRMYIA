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
    public class BannersDeOperadora : PageModel
    {
        public List<BannerOperadoraViewModel> ListBanner { get; set; }
        public List<Operadora> ListOperadora { get; set; }
        [BindProperty]
        public UsuarioCorretorViewModel UsuarioEntity { get; set; }
        //public IActionResult OnGet(string Id)
        //{

        //    if (Id.IsNullOrEmpty())
        //    {
        //        //
        //    }
        //    else
        //    {
        //        List<BannerOperadora> EntityBannerOperadora = null;
        //        List<long> IdBanner = new List<long>();
        //        EntityBannerOperadora = BannerOperadoraModel.GetAllBannerOperadora(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
        //        long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
        //        foreach (var Item in EntityBannerOperadora)
        //        {
        //            IdBanner.Add(Item.IdBanner.ToString().ExtractLong());
        //        }
        //        UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);
        //        ListBanner = BannerOperadoraModel.GetAllBanner(IdBanner);
        //        ListOperadora = OperadoraModel.GetList();
        //    }

        //    return Page();
        //}

        public IActionResult OnPostListarBannersOperadora()
        {
            string Id = Request.Form["Id"].ToString();

            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            bool status = false;
            Usuario EntityUsuario = null;
            EntityUsuario = UsuarioModel.Get(IdUsuario);
            int i;

            long? IdBanner = 0;
            //long? IdOperadora = 0;
            string IdCampanha = "";
            string Descricao = "";
            string Titulo = "";
            string NomeCampanha = "";
            string CaminhoArquivo = "";
            string NomeArquivo = "";
            string Width = "";
            string Height = "";
            string RedesSociais = "";
            string TipoPostagem = "";
            DateTime DataCadastro = DateTime.MinValue;
            bool Ativo = false;
            //Operadora IdOperadoraNavigation = new Operadora();
            Informacao IdInformacaoNavigation = new Informacao();

            List<long> IdBanners = new List<long>();
            List <BannerOperadoraViewModel> EntityBanners = null;
            List<BannerOperadoraCanvasViewModel> AuxBannerOperadoraCanvas = new List<BannerOperadoraCanvasViewModel>();

            Operadora EntityBannerOperadora = OperadoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            List<Informacao> EntityInformacao = InformacaoModel.Get();

            //foreach (var Item in EntityBannerOperadora)
            //{
            //    IdBanners.Add(Item.IdBanner.ToString().ExtractLong());
            //}
            UsuarioCorretorViewModel UsuarioEntity = UsuarioModel.GetUsuarioCorretor(IdUsuario);

            EntityBanners = BannerOperadoraModel.GetAllBanner(IdBanners, (byte)EntityUsuario.IdGrupoCorretor);

            foreach (var ItemInformacao in EntityInformacao)
            {
                i = 0;
                foreach (var ItemCampanhaArquivo in EntityBanners)
                {
                    if (ItemInformacao.IdInformacao == ItemCampanhaArquivo.IdInformacaoNavigation.IdInformacao)
                    {

                        if (i + 1 < EntityBanners.Count)
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
                        IdBanner = ItemCampanhaArquivo.IdBanner.ExtractLong();
                        IdCampanha = ItemCampanhaArquivo.IdCampanha;
                        Titulo = ItemCampanhaArquivo.IdInformacaoNavigation.Titulo;
                        Descricao = ItemCampanhaArquivo.IdInformacaoNavigation.Descricao;
                        CaminhoArquivo = ItemCampanhaArquivo.CaminhoArquivo;
                        NomeCampanha = ItemCampanhaArquivo.NomeCampanha;
                        //RedesSociais = ItemCampanhaArquivo.RedesSociais;
                        //TipoPostagem = ItemCampanhaArquivo.TipoPostagem;
                        DataCadastro = Convert.ToDateTime(ItemCampanhaArquivo.DataCadastro);
                        Ativo = ItemCampanhaArquivo.Ativo;
                        IdInformacaoNavigation = ItemCampanhaArquivo.IdInformacaoNavigation;
                        i++;
                    }

                }

                if (i > 0)
                {
                    AuxBannerOperadoraCanvas.Add(new BannerOperadoraCanvasViewModel()
                    {
                        IdOperadora = EntityBannerOperadora.IdOperadora,
                        IdCampanha = IdCampanha,
                        NomeOperadora = EntityBannerOperadora.Descricao,
                        NomeCampanha = NomeCampanha,
                        IdBanner = IdBanner,
                        Titulo = Titulo,
                        Descricao = Descricao,
                        CaminhoArquivo = CaminhoArquivo,
                        NomeArquivo = NomeArquivo,
                        CaminhoArquivoOperadora = EntityBannerOperadora.CaminhoArquivo,
                        NomeArquivoOperadora = EntityBannerOperadora.NomeArquivo,
                        Width = Width,
                        Height = Height,
                        DataCadastro = DataCadastro,
                        Ativo = Ativo,
                        IdInformacaoNavigation = IdInformacaoNavigation
                    });
                }
                NomeArquivo = "";
                Width = "";
                Height = "";

            }
            if (AuxBannerOperadoraCanvas.Count() > 0)
            {
                status = true;
            }
            return new JsonResult(new { status = status, campanhaArquivo = AuxBannerOperadoraCanvas, campanha = EntityInformacao, usuarioEntity = UsuarioEntity });
        }
    }
}
