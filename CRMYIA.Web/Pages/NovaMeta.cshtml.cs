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
        public Meta Entity { get; set; }
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
            CarregarLists();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                CarregarLists();

                if (Entity.IdMeta == 0 && KPIMetaValorEntity.IdKPIMetaValor == 0 && KPIMetaVidaEntity.IdKPIMetaVida == 0)
                {
                    MetaModel.Add(Entity);
                    KPIMetaValorEntity.IdMeta = Entity.IdMeta;
                    KPIMetaVidaEntity.IdMeta = Entity.IdMeta;

                    KPIMetaValorModel.Add(KPIMetaValorEntity);
                    KPIMetaVidaModel.Add(KPIMetaVidaEntity);

                }
                else
                {
                    MetaModel.Update(Entity);
                    KPIMetaValorModel.Update(KPIMetaValorEntity);
                    KPIMetaVidaModel.Update(KPIMetaVidaEntity);
                }
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
            ListUsuario = UsuarioModel.GetList();

        }
        #endregion

        #endregion
    }
}
