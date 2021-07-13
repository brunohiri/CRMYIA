using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class ListarCorretorModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<ListaCorretorViewModel> ListEntity { get; set; }
        #endregion

        #region Construtores
        public ListarCorretorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            ListEntity = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Corretor);
            foreach (var item in ListEntity)
            {
                var Supervisor = UsuarioHierarquiaModel.GetMaster(item.IdUsuario);
                if(Supervisor != null)
                {
                    item.Supervisor = Supervisor.IdUsuarioMasterNavigation.Nome;
                    var Gerente = UsuarioHierarquiaModel.GetMaster(Supervisor.IdUsuarioMasterNavigation.IdUsuario);
                    if(Gerente != null)
                    {
                        item.Gerente = Gerente.IdUsuarioMasterNavigation.Nome;
                    }
                    else
                    {
                        item.Gerente = "Nenhum";
                    }
                }
                else
                {
                    item.Supervisor = "Nenhum";
                }
                var UltimaProducao = PropostaModel.GetUltimaProducao(item.IdUsuario);
                if(UltimaProducao != null)
                {
                    item.UltimaProducao = UltimaProducao.DataCadastro;
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        #endregion
    }
}