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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CRMYIA.Web.Pages
{
    public class NovaCampanhaGenerica : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Campanha Entity { get; set; }
        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }

        #endregion

        #region Construtores
        public NovaCampanhaGenerica(IConfiguration configuration)
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
            CarregarLists();
            return Page();
        }

        public IActionResult OnPostListCampanha()
        {
            bool status = false;
           List<Campanha> ListCampanha =  Business.CampanhaModel.GetList();
            if(ListCampanha != null)
            {
                status = true;
            }
            return new JsonResult(new { status = status, retorno = ListCampanha });
        }

        public IActionResult OnPostSalvar([FromBody] Campanha dados)
        {
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            dados.DataCadastro = DateTime.Now;
            bool status = false;

            try
            {
                if (dados.IdCampanha == 0)
                {
                    dados.IdUsuario = IdUsuario;
                    dados.DataCadastro = DateTime.Now; 
                    dados.Url = Business.Util.Util.GetSlug(dados.Descricao);
                    Business.CampanhaModel.Add(dados);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    status = true;
                }
                else
                {
                    dados.IdUsuario = IdUsuario;
                    dados.DataCadastro = DateTime.Now;
                    Business.CampanhaModel.Update(dados);
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados atualizado com sucesso!");
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return new JsonResult(new { status = status });
            //return Page();
        }

        public IActionResult OnPostGetSubCategoria([FromBody] Campanha obj)
        {
            List<Campanha> Entity = null;
            bool status = false;
            if (obj.IdCampanhaReferencia.ToString() != null)
            {
                Entity = Business.CampanhaModel.GetSubCategoria(obj.IdCampanhaReferencia.ToString());
                if(Entity.Count() > 0)
                {
                    status = true;
                }
                
            }
            return new JsonResult(new { status = status, retorno = Entity });
            //return Page();
        }
        public void CarregarLists()
        {
            ListCampanha = Business.CampanhaModel.GetList();
        }

        #endregion
    }
}
