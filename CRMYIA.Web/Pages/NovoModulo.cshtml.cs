using System;
using System.Collections.Generic;
using System.Linq;
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
    public class NovoModuloModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
                                                                
        [BindProperty]
        public Modulo Entity { get; set; }

        [BindProperty]
        public List<Modulo> ListModuloReferencia { get; set; }

        #endregion

        #region Construtores
        public NovoModuloModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Modulo();
            else
            {
                Entity = ModuloModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            }

            Entity.DataCadastro = DateTime.Now;
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                ListModuloReferencia = ModuloModel.GetListIdDescricao();

                if (Entity.IdModulo == 0)
                    ModuloModel.Add(Entity);
                else
                    ModuloModel.Update(Entity);

                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion
    }
}
