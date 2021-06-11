using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ListarClienteModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public long? IdOrigem { get; set; }
        [BindProperty]
        public long? IdUsuario { get; set; }
        [BindProperty]
        public List<ListaClienteViewModel> ListEntity { get; set; }

        [BindProperty]
        public List<Origem> ListOrigemEntity { get; set; }

        [BindProperty]
        public List<Cliente> ListClienteEntity { get; set; }
        [BindProperty]
        public List<Cidade> ListCidadeEntity { get; set; }
        #endregion

        #region Construtores
        public ListarClienteModel(IConfiguration configuration)
        {                   
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            DateTime DataInicio = new DateTime(2020, 01, 01);
            DateTime DataFim = DateTime.Now;
            ListEntity = ClienteModel.GetList(null, null, null, null, DataInicio, DataFim);
            CarregarLists();
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }

        public IActionResult OnGetObter()
        {
            List<ListaClienteViewModel> ListaClienteEntity = null;

            ListaClienteEntity = ClienteModel.GetList(null, null, null, null, new DateTime(2020,01,01), DateTime.Now);

            return new JsonResult(new
            {
                draw = 4,
                recordsTotal = 57,
                recordsFiltered = 57,
                data = ListaClienteEntity,
                status = ListaClienteEntity.Count > 0
            });
        }

        public IActionResult OnPostObterCliente()
        {

            return new JsonResult(new
            {
                data = ClienteModel.GetList()
            });
        }

        public IActionResult OnPostPesquisa(ClienteViewModel dados)
        {
            List<ListaClienteViewModel> ListaClienteEntity = null;

            if (dados.DataInicio == DateTime.MinValue && dados.DataFim == DateTime.MinValue)
            {
                dados.DataInicio = null;
                dados.DataFim = null;
            }

            ListaClienteEntity = ClienteModel.GetList(dados.StatusPlanoLead, dados.IdOrigem, dados.Nome, dados.NomeCidade, dados.DataInicio, dados.DataFim);

            return new JsonResult(new
            {
                lista = ListaClienteEntity,
                status = ListaClienteEntity.Count > 0
            });
        }

        public long GetIdUsuario()
        {
            long IdUsuario = "0".ExtractLong();

            if (HttpContext.User.Equals("IdUsuarioSlave"))
            {
                IdUsuario = HttpContext.User.FindFirst("IdUsuarioSlave").Value.ExtractLong();
            }
            else
            {
                IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            }
            return IdUsuario;
        }

        public void CarregarLists()
        {
            byte? IdPerfil = 0;
            long IdUsuario = GetIdUsuario();

            ListOrigemEntity = OrigemLeadModel.GetList();

            ListClienteEntity = ClienteModel.GetList();

            ListCidadeEntity = CidadeModel.GetList();
        }

        #endregion
    }
}
