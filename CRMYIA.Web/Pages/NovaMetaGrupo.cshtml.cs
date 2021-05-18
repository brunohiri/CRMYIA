using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class NovaMetaGrupoModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        #region Meta
        [BindProperty]
        public KPIMeta Entity { get; set; }
        [BindProperty]
        public KPIGrupo EntityKPIGrupo { get; set; }

        #endregion

        #region KPIMetaValor
        [BindProperty]
        public KPIMetaValor KPIMetaValorEntity { get; set; }
        #endregion

        #region KPIMetaVida
        [BindProperty]
        public KPIMetaVida KPIMetaVidaEntity { get; set; }
        #endregion

        #region Usuario
        [BindProperty]
        public List<Usuario> ListUsuario { get; set; }
        #endregion
        public long idUser { get; set; }
        public int Mes { get; set; }
        public string _name { get; set; }
        #endregion

        #region Construtores
        public NovaMetaGrupoModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {

            if (Id.IsNullOrEmpty())
            {
                Entity = new KPIMeta();
            }
            else
            {
                idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();

                Mes = DateTime.Now.Month;

                EntityKPIGrupo = KPIGrupoModel.Get(idUser);
                Entity = KPIMetaModel.Get((long)EntityKPIGrupo.IdKPIGrupo);
                _name = EntityKPIGrupo.Nome;
                if (Entity != null)
                {
                    KPIMetaValorEntity = KPIMetaValorModel.Get(Entity.IdMeta);
                    KPIMetaVidaEntity = KPIMetaVidaModel.Get(Entity.IdMeta);
                }
                else
                {
                    Entity = new KPIMeta();
                }
            }
            //CarregarLists();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                if (Entity.Ativo == false)
                {
                    KPIMetaValorEntity.Ativo = false;
                    KPIMetaVidaEntity.Ativo = false;
                }
                else
                {
                    KPIMetaValorEntity.Ativo = true;
                    KPIMetaVidaEntity.Ativo = true;
                }
                if (Entity.IdMeta == 0 && KPIMetaValorEntity.IdKPIMetaValor == 0 && KPIMetaVidaEntity.IdKPIMetaVida == 0)
                {

                    Entity.IdKPIGrupo = EntityKPIGrupo.IdKPIGrupo;
                    KPIMetaModel.Add(Entity);
                    KPIMetaValorEntity.IdMeta = Entity.IdMeta;
                    KPIMetaVidaEntity.IdMeta = Entity.IdMeta;

                    KPIMetaValorModel.Add(KPIMetaValorEntity);
                    KPIMetaVidaModel.Add(KPIMetaVidaEntity);
                }
                else
                {
                    Entity.IdKPIGrupo = EntityKPIGrupo.IdKPIGrupo;
                    Entity.DataMinima = DateTime.Parse(Request.Form["DataMinima"]);
                    Entity.DataMaxima = DateTime.Parse(Request.Form["DataMaxima"]);
                    KPIMetaValorEntity.IdMeta = Entity.IdMeta;
                    KPIMetaVidaEntity.IdMeta = Entity.IdMeta;
                    
                    KPIMetaModel.Update(Entity);
                    KPIMetaValorModel.Update(KPIMetaValorEntity);
                    KPIMetaVidaModel.Update(KPIMetaVidaEntity);
                }
                EntityKPIGrupo = KPIGrupoModel.GetByKPIGrupo((long)Entity.IdKPIGrupo);
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
                EntityKPIGrupo = new KPIGrupo();
                EntityKPIGrupo.Nome = _name;
            }
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
