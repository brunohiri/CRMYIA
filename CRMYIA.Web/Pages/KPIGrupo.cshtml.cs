using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class KPIGruposModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;


        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public KPIGrupo KPIGrupoEntity { get; set; }
        [BindProperty]
        public List<ListaKPIUsuarioViewModel> ListCorretor { get; set; }
        [BindProperty]
        public List<ListaKPIUsuarioViewModel> ListSupervisor { get; set; }
        [BindProperty]
        public KPIUsuarioViewModel UsuarioCargo { get; set; }
        [BindProperty]
        public List<KPIGrupo> ListKPIGrupo { get; set; }
        [BindProperty]
        public List<KPIGrupoUsuario> ListKPIGrupoUsuario { get; set; }
        [BindProperty]
        public List<KPIMeta> ListKPIMeta { get; set; }
        [BindProperty]
        public List<KPIMetaValor> ListKPIMetaValor { get; set; }
        [BindProperty]
        public List<KPIMetaVida> ListKPIMetaVida { get; set; }
        [BindProperty]
        public List<KPIMetaIndividual> ListKPIMetaIndividual { get; set; }
        [BindProperty]
        public List<KPIMetaValorIndividual> ListKPIMetaValorIndividual { get; set; }
        [BindProperty]
        public List<KPIMetaVidaIndividual> ListKPIMetaVidaIndividual { get; set; }
        [BindProperty]
        public List<ListKPIRealizadoPropostaViewModel> ListKPIRealizadoProposta { get; set; }
        public List<List<ListKPIRealizadoPropostaViewModel>> listAllRealizado { get; set; }

        public int? IdPerfil { get; set; }
        public decimal? Total { get; set; }
        #endregion

        #region Construtores
        public KPIGruposModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            CarregarLists();
            KPIGrupoEntity = new KPIGrupo();
            return Page();
        }
        public IActionResult OnPost()
        {
            CarregarLists();
            return Page();
        }
        public IActionResult OnPostSearchByName(IFormCollection dados)
        {
            int start = int.Parse(dados["start"]);
            string nome = dados["termo"];
            int take = int.Parse(dados["take"]);

            ListCorretor = UsuarioModel.GetListKPIUsuarioByName((byte)(EnumeradorModel.Perfil.Corretor), "Corretor", nome).Select(x => new ListaKPIUsuarioViewModel
            {
                IdUsuario = x.IdUsuario,
                Nome = x.Nome,
                NomeFoto = x.NomeFoto,
                CaminhoFoto = x.CaminhoFoto,
                DataCadastro = x.DataCadastro,
                Corretora = x.Corretora,
                DescricaoPerfil = x.DescricaoPerfil,
                Ativo = x.Ativo,
                IdClassificacaoNavigation = x.IdClassificacaoNavigation
            }).Skip(start).Take(take).ToList();

            return new JsonResult(new { ListCorretor = ListCorretor, start = start == 0 ? 100 : start });
        }
        public IActionResult OnPostCorretores(IFormCollection dados)
        {
            int start = int.Parse(dados["start"]);

            ListCorretor = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Corretor), "Corretor").Select(x => new ListaKPIUsuarioViewModel
            {
                IdUsuario = x.IdUsuario,
                Nome = x.Nome,
                NomeFoto = x.NomeFoto,
                CaminhoFoto = x.CaminhoFoto,
                DataCadastro = x.DataCadastro,
                Corretora = x.Corretora,
                DescricaoPerfil = x.DescricaoPerfil,
                Ativo = x.Ativo,
                IdClassificacaoNavigation = x.IdClassificacaoNavigation
            }).Skip(start).Take(20).ToList();

            return new JsonResult(new { ListCorretor = ListCorretor });
        }
        public IActionResult OnGetEdit(string grupoId = null, string usuarioId = null, string metaId = null)
        {
            if ((!grupoId.IsNullOrEmpty()) && (!usuarioId.IsNullOrEmpty()) && (!metaId.IsNullOrEmpty()))
            {
                KPIGrupoUsuario EntityKPIGrupoUsuario = KPIGrupoUsuarioModel.Get(usuarioId.ExtractLong());

                if (EntityKPIGrupoUsuario != null)
                {
                    EntityKPIGrupoUsuario.IdKPIGrupo = grupoId.ExtractLong();
                    EntityKPIGrupoUsuario.IdUsuario = usuarioId.ExtractLong();
                    EntityKPIGrupoUsuario.IdMeta = metaId.ExtractLong();


                    KPIGrupoUsuarioModel.Update(EntityKPIGrupoUsuario);
                }
                else
                {
                    UsuarioCargo = UsuarioModel.GetKPIUsuario(usuarioId.ExtractLong());
                    EntityKPIGrupoUsuario = new KPIGrupoUsuario();

                    EntityKPIGrupoUsuario.IdKPIGrupo = grupoId.ExtractLong();
                    EntityKPIGrupoUsuario.IdUsuario = usuarioId.ExtractLong();
                    EntityKPIGrupoUsuario.IdMeta = metaId.ExtractLong();
                    EntityKPIGrupoUsuario.Inicio = DateTime.Now;
                    EntityKPIGrupoUsuario.Nome = UsuarioCargo.Nome;
                    EntityKPIGrupoUsuario.Perfil = UsuarioCargo.DescricaoPerfil;
                    EntityKPIGrupoUsuario.NomeFoto = UsuarioCargo.NomeFoto;
                    EntityKPIGrupoUsuario.CaminhoFoto = UsuarioCargo.CaminhoFoto;
                    EntityKPIGrupoUsuario.Grupo = true;
                    EntityKPIGrupoUsuario.Ativo = true;


                    //Notificacao EntityNotificacao = NotificacaoModel.Add(new Notificacao()
                    //{
                    //    IdUsuarioCadastro = IdUsuario,
                    //    IdUsuarioVisualizar = EntityUsuarioHierarquia.IdUsuarioMaster,
                    //    Titulo = null,
                    //    Descricao = dados.Descricao,
                    //    Url = "/Visita?Id=" + HttpUtility.UrlEncode(Criptography.Encrypt(EntityVisita.IdVisita.ToString())),
                    //    Visualizado = false,
                    //    DataCadastro = DateTime.Now,
                    //    Ativo = true
                    //});

                    try
                    {
                        KPIGrupoUsuarioModel.Add(EntityKPIGrupoUsuario);
                    }
                    catch
                    {
                        return new JsonResult(new { status = false });
                    }
                }
            }
            return new JsonResult(new { status = true });
        }
        public IActionResult OnPostKPIGrupo()
        {
            try
            {
                var req = Request.Form;
                if (KPIGrupoEntity.IdKPIGrupo == 0)
                {
                    KPIGrupoEntity.DataCadastro = DateTime.Now;
                    KPIGrupoModel.Add(KPIGrupoEntity);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    ListKPIGrupo = KPIGrupoModel.GetList();
                }
                else if (KPIGrupoEntity.Nome.Length <= 0)
                {
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Verifique os campos!");
                }
                else
                {
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Não foi possivel salvar!");
                }
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            CarregarLists();
            return new JsonResult(new { mensagem = Mensagem });
        }
        public IActionResult OnPostExcluirKPIGrupo(IFormCollection dados)
        {
            int id;
            bool status = false;
            KPIGrupo user = new KPIGrupo();

            id = int.Parse(dados["id"]);
            user.IdKPIGrupo = id;

            if (id > 0)
            {
                KPIGrupoModel.Excluir(user);
                status = true;
            }
            if (status == false)
                return new JsonResult(new { mensagem = "Erro ao excluir o cartão!", status });
            else
                return new JsonResult(new { mensagem = "Sucesso", status });
        }
        public IActionResult OnPostExcluirKPIGrupoUsuario(IFormCollection dados)
        {
            int id = 0;
            string motivo = "";
            bool status = false;
            KPIGrupoUsuario user = new KPIGrupoUsuario();

            id = int.Parse(dados["id"]);
            motivo = dados["motivo"];
            user.IdKPIGrupoUsuario = id;
            user.Motivo = motivo;

            if (id > 0)
            {
                KPIGrupoUsuarioModel.Excluir(user);
                status = true;
            }
            if (status == false)
                return new JsonResult(new { mensagem = "Erro ao excluir o cartão!", status });
            else
                return new JsonResult(new { mensagem = "Sucesso", status });
        }
        #endregion


        public void CarregarLists()
        {
            ListKPIGrupo = KPIGrupoModel.GetList();
            ListKPIGrupoUsuario = KPIGrupoUsuarioModel.GetList();
            listAllRealizado = new List<List<ListKPIRealizadoPropostaViewModel>>();
            foreach (var item in ListKPIGrupoUsuario)
            {
                listAllRealizado.Add(PropostaModel.GetListKPIRealizadoProposta((long)item.IdUsuario, item.IdMetaNavigation.DataMinima, item.IdMetaNavigation.DataMaxima));
            }

            ListSupervisor = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Supervisor), "Supervisor");

            ListCorretor = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Corretor), "Corretor").Select(x => new ListaKPIUsuarioViewModel
            {
                IdUsuario = x.IdUsuario,
                Nome = x.Nome,
                NomeFoto = x.NomeFoto,
                CaminhoFoto = x.CaminhoFoto,
                DataCadastro = x.DataCadastro,
                Corretora = x.Corretora,
                DescricaoPerfil = x.DescricaoPerfil,
                Ativo = x.Ativo,
                IdClassificacaoNavigation = x.IdClassificacaoNavigation
            }).Take(20).ToList();
        }

    }
}