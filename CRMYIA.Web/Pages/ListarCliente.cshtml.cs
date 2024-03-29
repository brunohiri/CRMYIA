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

        [BindProperty]
        public List<ListaCorretorViewModel> ListCorretor { get; set; }
        #endregion

        #region Construtores
        public ListarClienteModel(IConfiguration configuration)
        {                   
            _configuration = configuration;
        }
        #endregion

        #region M�todos
        public IActionResult OnGet()
        {
            DateTime DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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

            DateTime DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ListaClienteEntity = ClienteModel.GetList(null, null, null, null, DataInicio, DataFim);

            return new JsonResult(new
            {
                draw = 4,
                recordsTotal = 57,
                recordsFiltered = 57,
                data = ListaClienteEntity,
                status = ListaClienteEntity.Count > 0
            });
        }

        public IActionResult OnPostVincularCorretor(EnviarCorretorViewModel dados)
        {
            List<ListaClienteViewModel> ListaClienteEntity = null;
            DateTime DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var Itens = dados.Itens.Split(',');

            foreach(var Item in Itens)
            {
                UsuarioClienteModel.Add(new UsuarioCliente
                {
                    IdUsuario = dados.IdUsuarioCorretor,
                    IdCliente = Item.ExtractLong(),
                    DataCadastro = DateTime.Now,
                    Ativo = true
                });
                UsuarioClienteModel.DesativarUltimoCorretor(Item.ExtractLong(), dados.IdUsuarioCorretor);
            }
            
            ListaClienteEntity = ClienteModel.GetList(null, null, null, null, DataInicio, DataFim);

            return new JsonResult(new
            {
                lista = ListaClienteEntity,
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
                dados.DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
                dados.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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

            ListCorretor = UsuarioModel.GetList((byte)(EnumeradorModel.Perfil.Corretor));
        }

        #endregion
    }
}
