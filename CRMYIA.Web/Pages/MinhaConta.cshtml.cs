using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class MinhaContaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        #region Usuario
        [BindProperty]
        public Usuario Entity { get; set; }
        [BindProperty]
        public Perfil EntityCargo { get; set; }
        public long IdUsuario { get; set; }
        public UsuarioPerfil UsuarioPerfil { get; set; }
        #endregion
        #endregion

        #region Construtores
        public MinhaContaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            UsuarioPerfil = PerfilModel.GetIdentificacaoPerfil(IdUsuario);

            Entity = UsuarioModel.Get(IdUsuario);
            EntityCargo = PerfilModel.Get(UsuarioPerfil.IdPerfil.Value);
            //if (Id.IsNullOrEmpty())
            //{
            //    Entity = new KPIMeta();
            //}
            //else
            //{
            //    idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();

            //    Mes = DateTime.Now.Month;

            //    EntityKPIGrupo = KPIGrupoModel.Get(idUser);
            //    Entity = KPIMetaModel.Get((long)EntityKPIGrupo.IdKPIGrupo);
            //    if (Entity != null)
            //    {
            //        KPIMetaValorEntity = KPIMetaValorModel.Get(Entity.IdMeta);
            //        KPIMetaVidaEntity = KPIMetaVidaModel.Get(Entity.IdMeta);
            //    }
            //    else
            //    {
            //        Entity = new KPIMeta();
            //    }
            //}
            //CarregarLists();
            return Page();
        }
        public IActionResult OnPostRedesSociais(IFormCollection dados)
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
        public IActionResult OnPostAlterarSenha(IFormCollection dados)
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
        public IActionResult OnPostAlterarDados(IFormCollection dados)
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
        public IActionResult OnPost()
        {
            //try
            //{
            //    if (Entity.Ativo == false)
            //    {
            //        KPIMetaValorEntity.Ativo = false;
            //        KPIMetaVidaEntity.Ativo = false;
            //    }
            //    else
            //    {
            //        KPIMetaValorEntity.Ativo = true;
            //        KPIMetaVidaEntity.Ativo = true;
            //    }
            //    if (Entity.IdMeta == 0 && KPIMetaValorEntity.IdKPIMetaValor == 0 && KPIMetaVidaEntity.IdKPIMetaVida == 0)
            //    {

            //        Entity.IdKPIGrupo = EntityKPIGrupo.IdKPIGrupo;
            //        if (DateTime.TryParse(Request.Form["DataMinima"], out DateTime data))
            //        {
            //            Entity.DataMinima = DateTime.Parse(Request.Form["DataMinima"]);
            //            Entity.DataMaxima = DateTime.Parse(Request.Form["DataMaxima"]);
            //        }
            //        else
            //        {
            //            Entity.DataMinima = DateTime.Parse(Request.Form["DtInicio"]);
            //            Entity.DataMaxima = DateTime.Parse(Request.Form["DtFinal"]);
            //        }
            //        KPIMetaModel.Add(Entity);
            //        KPIMetaValorEntity.IdMeta = Entity.IdMeta;
            //        KPIMetaVidaEntity.IdMeta = Entity.IdMeta;

            //        KPIMetaValorModel.Add(KPIMetaValorEntity);
            //        KPIMetaVidaModel.Add(KPIMetaVidaEntity);
            //    }
            //    else
            //    {
            //        Entity.IdKPIGrupo = EntityKPIGrupo.IdKPIGrupo;

            //        if(DateTime.TryParse(Request.Form["DataMinima"], out DateTime data))
            //        {
            //            Entity.DataMinima = DateTime.Parse(Request.Form["DataMinima"]);
            //            Entity.DataMaxima = DateTime.Parse(Request.Form["DataMaxima"]);
            //        }
            //        else
            //        {
            //            Entity.DataMinima = DateTime.Parse(Request.Form["DtInicio"]);
            //            Entity.DataMaxima = DateTime.Parse(Request.Form["DtFinal"]);
            //        }
            //        KPIMetaValorEntity.IdMeta = Entity.IdMeta;
            //        KPIMetaVidaEntity.IdMeta = Entity.IdMeta;
                    
            //        KPIMetaModel.Update(Entity);
            //        KPIMetaValorModel.Update(KPIMetaValorEntity);
            //        KPIMetaVidaModel.Update(KPIMetaVidaEntity);
            //    }
            //    EntityKPIGrupo = KPIGrupoModel.GetByKPIGrupo((long)Entity.IdKPIGrupo);
            //    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            //}
            //catch (Exception ex)
            //{
            //    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            //}
            return Page();
        }

        #region Outros Métodos
        private void CarregarLists()
        {
            //ListUsuario = UsuarioModel.GetList();

        }
        #endregion

        #endregion
    }
}
