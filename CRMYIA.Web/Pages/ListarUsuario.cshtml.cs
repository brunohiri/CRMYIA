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
    public class ListarUsuarioModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<UsuarioViewModel> ListEntity { get; set; }
        [BindProperty]
        public List<Perfil> ListPerfilEntity { get; set; }
        #endregion

        #region Construtores
        public ListarUsuarioModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet()
        {
            DateTime DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
            DateTime DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ListEntity = UsuarioModel.GetList(null, null, DataInicio, DataFim, false);
            ListPerfilEntity = PerfilModel.GetList();
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
        public IActionResult OnPostPesquisa(EnviarUsuarioViewModel dados)
        {
            List<UsuarioViewModel> ListaUsuarioEntity = null;
            bool DataValida = true;
            if (dados.DataInicio == DateTime.MinValue && dados.DataFim == DateTime.MinValue)
            {
                dados.DataInicio = new DateTime(DateTime.Now.AddDays(-120).Year, DateTime.Now.AddDays(-120).Month, DateTime.Now.AddDays(-120).Day);
                dados.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DataValida = false;
            }

            if(dados.Descricao == "Selecione...")
            {
                dados.Descricao = null;
            }

            ListaUsuarioEntity = UsuarioModel.GetList(dados.Ativo, dados.Descricao, dados.DataInicio, dados.DataFim, DataValida);

            return new JsonResult(new
            {
                lista = ListaUsuarioEntity.Count > 0 ? ListaUsuarioEntity : null ,
                status = ListaUsuarioEntity.Count > 0
            });
        }
        #endregion
    }
}
