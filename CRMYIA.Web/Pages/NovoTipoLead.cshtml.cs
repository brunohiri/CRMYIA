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
    public class NovoTipoLeadModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<KPIServico> ListKPIServico { get; set; }
        [BindProperty]
        public List<KPICargo> ListKPICargo { get; set; }
        [BindProperty]
        public TipoLead Entity { get; set; }

        #endregion

        #region Construtores
        public NovoTipoLeadModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new TipoLead();
            else
                Entity =TipoLeadModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());


            CarregarLists();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                if (Entity.IdTipoLead == 0)
                    TipoLeadModel.Add(Entity);
                else
                    TipoLeadModel.Update(Entity);

                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion
        public void CarregarLists()
        {
            ListKPIServico = KPIServicoModel.GetList();
        }
    }
}
