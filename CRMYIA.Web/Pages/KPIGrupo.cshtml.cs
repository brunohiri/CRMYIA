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
        public List<ListaKPIUsuarioViewModel> ListPerfil { get; set; }
        [BindProperty]
        public KPIUsuarioViewModel UsuarioCargo { get; set; }
        [BindProperty]
        public List<KPIGrupo> ListKPIGrupo { get; set; }
        [BindProperty]
        public List<KPIGrupoUsuario> ListKPIGrupoUsuario { get; set; }
        [BindProperty]
        public int? IdPerfil { get; set; }
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
            CarregarLists(1);
            KPIGrupoEntity = new KPIGrupo();
            return Page();
        }
        public IActionResult OnPost()
        {
            int id = 0;
            id = int.Parse(Request.Form["IdPerfil"]);
            if (id > 0)
                CarregarLists(int.Parse(Request.Form["IdPerfil"]));
            return Page();
        }
        public IActionResult OnGetEdit(string grupoId = null, string usuarioId = null)
        {
            if ((!grupoId.IsNullOrEmpty()) && (!usuarioId.IsNullOrEmpty()))
            {
                KPIGrupoUsuario EntityKPIGrupoUsuario = KPIGrupoUsuarioModel.Get(usuarioId.ExtractLong());

                if (EntityKPIGrupoUsuario != null)
                {
                    EntityKPIGrupoUsuario.IdKPIGrupo = grupoId.ExtractLong();
                    EntityKPIGrupoUsuario.IdUsuario = usuarioId.ExtractLong();

                    KPIGrupoUsuarioModel.Update(EntityKPIGrupoUsuario);
                }
                else
                {
                    UsuarioCargo = UsuarioModel.GetKPIUsuario(usuarioId.ExtractLong());
                    EntityKPIGrupoUsuario = new KPIGrupoUsuario();

                    EntityKPIGrupoUsuario.IdKPIGrupo = grupoId.ExtractLong();
                    EntityKPIGrupoUsuario.IdUsuario = usuarioId.ExtractLong();
                    EntityKPIGrupoUsuario.Inicio = DateTime.Now;
                    EntityKPIGrupoUsuario.Nome = UsuarioCargo.Nome;
                    EntityKPIGrupoUsuario.Perfil = UsuarioCargo.DescricaoPerfil;
                    EntityKPIGrupoUsuario.NomeFoto = UsuarioCargo.NomeFoto;
                    EntityKPIGrupoUsuario.CaminhoFoto = UsuarioCargo.CaminhoFoto;
                    EntityKPIGrupoUsuario.Grupo = true;
                    EntityKPIGrupoUsuario.Ativo = true;
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
        #endregion

        public void CarregarLists(int perfil = 0)
        {
            ListKPIGrupo = KPIGrupoModel.GetList();
            ListKPIGrupoUsuario = KPIGrupoUsuarioModel.GetList();
            if (perfil > 0)
            {
                if (perfil == 1)
                    ListPerfil = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Gerente), "Gerente");
                if (perfil == 2)
                    ListPerfil = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Supervisor), "Supervisor");
                //if (perfil == 3)
                //    ListPerfil = UsuarioModel.GetListKPIUsuario((byte)(EnumeradorModel.Perfil.Corretor), "Corretor");
            }

        }

    }
}
