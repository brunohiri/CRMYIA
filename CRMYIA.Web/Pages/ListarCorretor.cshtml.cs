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

        [BindProperty]
        public string FindCorretor { get; set; }
        public string DataInicial;
        public string DataFinal;
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
            CarregarLists();
            Supervisor = new UsuarioHierarquia();
            Gerente = new UsuarioHierarquia();
            ListEntity = UsuarioModel.GetListSuperior((byte)EnumeradorModel.Perfil.Corretor);

            return Page();
        }

        public IActionResult OnPost()
        {
            string Corretor = Request.Form["FindCorretor"];
            long Corretora = long.Parse(Request.Form["IdCorretora"]);
            long Supervisor = Request.Form.Keys.Contains("IdSupervisor") ? long.Parse(Request.Form["IdSupervisor"]) : 0;
            long Gerente = Request.Form.Keys.Contains("IdGerente") ? long.Parse(Request.Form["IdGerente"]) : 0;
            string DataInicial = Request.Form["DataInicial"];
            string DataFinal = Request.Form["DataFinal"];
            bool Status = bool.Parse(Request.Form["Status"]);

            ListEntity = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Corretor, Corretor, Corretora, Supervisor, Gerente, DataInicial, DataFinal, Status);
            CarregarLists();
            return Page();
        }
        #endregion
        public void CarregarLists()
        {
            Status = true;
            DataInicial = DateTime.Now.ToString();
            DataFinal = DateTime.Now.AddMonths(1).ToString();
            ListSupervisor = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Supervisor);
            ListGerente = UsuarioModel.GetList((byte)EnumeradorModel.Perfil.Gerente);
            ListCorretora = CorretoraModel.GetList();
        }
    }
}