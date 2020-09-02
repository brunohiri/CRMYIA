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
    public class NovoCorretoraModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Corretora Entity { get; set; }

        [BindProperty]
        public List<Cidade> ListCidade { get; set; }

        [BindProperty]
        public List<Estado> ListEstado { get; set; }

        #endregion

        #region Construtores
        public NovoCorretoraModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Corretora();
            else
                Entity = CorretoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());

            Entity.DataCadastro = DateTime.Now;
            ListEstado = EstadoModel.GetListIdSigla();
            ListCidade = CidadeModel.GetListIdDescricao();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                ListEstado = EstadoModel.GetListIdSigla();
                ListCidade = CidadeModel.GetListIdDescricao();

                if (!Util.IsCnpj(Entity.CNPJ))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CNPJ Inválido!");
                else
                {
                    if (Entity.IdCorretora == 0)
                        CorretoraModel.Add(Entity);
                    else
                        CorretoraModel.Update(Entity);

                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
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
