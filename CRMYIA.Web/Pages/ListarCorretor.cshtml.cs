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
        [BindProperty]
        public UsuarioHierarquia Gerente { get; set; }
        [BindProperty]
        public UsuarioHierarquia Supervisor { get; set; }
        [BindProperty]
        public List<Corretora> ListCorretora { get; set; }
        [BindProperty]
        public List<ListaCorretorViewModel> ListGerente { get; set; }
        [BindProperty]
        public List<ListaCorretorViewModel> ListSupervisor { get; set; }
        [BindProperty]
        public bool Status { get; set; }
        [BindProperty]
        public string Corretor { get; set; }
        [BindProperty]
        public bool IdCorretora { get; set; }
        [BindProperty]
        public bool IdSupervisor { get; set; }
        [BindProperty]
        public bool IdGerente { get; set; }
        #endregion

        #region Construtores
        public ListarCorretorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            CarregarLists();
            Supervisor = new UsuarioHierarquia();
            Gerente = new UsuarioHierarquia();
            ListEntity = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Corretor);
            
            //foreach (var item in ListEntity)
            //{
            //     Supervisor = UsuarioHierarquiaModel.GetMaster(item.IdUsuario);
            //    if(Supervisor != null)
            //    {
            //        item.Supervisor = Supervisor.IdUsuarioMasterNavigation.Nome;
            //         Gerente = UsuarioHierarquiaModel.GetMaster(Supervisor.IdUsuarioMasterNavigation.IdUsuario);
            //        if(Gerente != null)
            //        {
            //            item.Gerente = Gerente.IdUsuarioMasterNavigation.Nome;
            //        }
            //        else
            //        {
            //            item.Gerente = "Nenhum";
            //        }
            //    }
            //    else
            //    {
            //        item.Supervisor = "Nenhum";
            //    }
            //    var UltimaProducao = PropostaModel.GetUltimaProducao(item.IdUsuario);
            //    if(UltimaProducao != null)
            //    {
            //        item.UltimaProducao = UltimaProducao.DataCadastro;
            //    }
            //}
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        #endregion
        public void CarregarLists()
        {
            ListSupervisor = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Supervisor);
            ListGerente = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Gerente);
        }
    }
}