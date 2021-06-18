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
    public class NovoGrupoCorretorModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public GrupoCorretor Entity { get; set; }

        #endregion

        #region Construtores
        public NovoGrupoCorretorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new GrupoCorretor();
            else
            {
                Entity = GrupoCorretorModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            }

            return Page();
        }

        public IActionResult OnPostSalvar(GrupoCorretor dados)
        {
            MensagemModel mensagem = null;
            try
            {
                if (dados.IdGrupoCorretor == 0)
                {
                    dados.DataCadastro = DateTime.Now;
                    GrupoCorretorModel.Add(dados);
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                }
                else
                {
                    dados.DataCadastro = DateTime.Now;
                    GrupoCorretorModel.Update(dados);
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterado com sucesso!");
                }


            }
            catch (Exception ex)
            {
                mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion
    }
}
