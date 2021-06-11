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
    public class NovaMetaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        #region Meta
        [BindProperty]
        public KPIMetaIndividual Entity { get; set; }
        [BindProperty]
        public KPIGrupoUsuario EntityKPIGrupoUsuario { get; set; }

        #endregion

        #region KPIMetaValor
        [BindProperty]
        public KPIMetaValorIndividual KPIMetaValorEntity { get; set; }
        #endregion

        #region KPIMetaVida
        [BindProperty]
        public KPIMetaVidaIndividual KPIMetaVidaEntity { get; set; }
        #endregion

        #region Usuario
        [BindProperty]
        public List<Usuario> ListUsuario { get; set; }
        #endregion
        public string origem { get; set; }
        public long idUser { get; set; }
        #endregion

        #region Construtores
        public NovaMetaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {

            if (Id.IsNullOrEmpty())
            {
                Entity = new KPIMetaIndividual();
            }
            else
            {
                origem = HttpContext.Request.Headers["Referer"].ToString();
                idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();

                if (origem.Contains("KPIGrupo"))
                {
                    EntityKPIGrupoUsuario = KPIGrupoUsuarioModel.Get(idUser);
                    Entity = KPIMetaIndividualModel.Get((long)EntityKPIGrupoUsuario.IdKPIGrupoUsuario);
                    if (Entity != null)
                    {
                        KPIMetaValorEntity = KPIMetaValorIndividualModel.Get(Entity.IdMetaIndividual);
                        KPIMetaVidaEntity = KPIMetaVidaIndividualModel.Get(Entity.IdMetaIndividual);
                    }
                    else
                    {
                        Entity = new KPIMetaIndividual();
                    }
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
                if (Entity.IdMetaIndividual == 0 && KPIMetaValorEntity.IdKPIMetaValorIndividual == 0 && KPIMetaVidaEntity.IdKPIMetaVidaIndividual == 0)
                {

                    Entity.IdKPIGrupoUsuario = EntityKPIGrupoUsuario.IdKPIGrupoUsuario;
                    Entity.DataMaxima = DateTime.Now;
                    Entity.DataMinima = DateTime.Now;
                    KPIMetaIndividualModel.Add(Entity);
                    KPIMetaValorEntity.IdMetaIndividual = Entity.IdMetaIndividual;
                    KPIMetaVidaEntity.IdMetaIndividual = Entity.IdMetaIndividual;

                    KPIMetaValorIndividualModel.Add(KPIMetaValorEntity);
                    KPIMetaVidaIndividualModel.Add(KPIMetaVidaEntity);
                }
                else
                {
                    Entity.IdKPIGrupoUsuario = EntityKPIGrupoUsuario.IdKPIGrupoUsuario;
                    KPIMetaValorEntity.IdMetaIndividual = Entity.IdMetaIndividual;
                    KPIMetaVidaEntity.IdMetaIndividual = Entity.IdMetaIndividual;
                    KPIMetaIndividualModel.Update(Entity);
                    KPIMetaValorIndividualModel.Update(KPIMetaValorEntity);
                    KPIMetaVidaIndividualModel.Update(KPIMetaVidaEntity);
                }
                EntityKPIGrupoUsuario = KPIGrupoUsuarioModel.GetByKPIGrupoUsuario((long)Entity.IdKPIGrupoUsuario);
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
                
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
