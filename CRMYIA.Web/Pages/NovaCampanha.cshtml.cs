using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CRMYIA.Business;
using System.Security.Claims;
using CRMYIA.Business.Util;
using System.Web;

namespace CRMYIA.Web.Pages
{
    public class NovaCampanha : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Campanha Entity { get; set; }

        #endregion

        #region Construtores
        public NovaCampanha(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Campanha();
            else
                Entity = Business.CampanhaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());

            Entity.DataCadastro = DateTime.Now;
            return Page();
        }

        public IActionResult OnPost()
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            try
            {
                if (Entity.IdCampanha == 0)
                {
                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Business.CampanhaModel.Add(Entity);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                }
                else
                {
                    Entity.IdUsuario = IdUsuario;
                    Entity.DataCadastro = DateTime.Now;
                    Business.CampanhaModel.Update(Entity);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados atualizado com sucesso!");
                }
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
