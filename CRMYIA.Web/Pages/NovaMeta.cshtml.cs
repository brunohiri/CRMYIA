using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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
    public class NovaMetaModel : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public Meta Entity { get; set; }

        #region Usuario
        [BindProperty]
        public List<Usuario> ListUsuario { get; set; }
        #endregion

        #region KPIMetaValor
        [BindProperty]
        public List<KPIMetaValor> ListKPIMetaValor { get; set; }
        #endregion

        #region KPIMetaVida
        [BindProperty]
        public List<KPIMetaVida> ListKPIMetaVida { get; set; }
        #endregion

        #endregion

        #region Construtores
        public NovaMetaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            //if (Id.IsNullOrEmpty())
            //{
            //    Entity = new Cliente();
            //}
            //else
            //{
            //    Entity = ClienteModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            //}

            //Entity.DataCadastro = DateTime.Now;
            CarregarLists();

           

            return Page();
        }

        public IActionResult OnPost()
        {
            //try
            //{
            //    CarregarLists();

            //    if ((!Util.IsCpf(Entity.CPF)) && (!Util.IsCnpj(Entity.CPF)))
            //        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CPF ou CNPJ Inválido!");
            //    else
            //    {
            //        if (Entity.IdCliente == 0)
            //        {
            //            if (ClienteModel.GetByCPF(Entity.CPF) != null)
            //                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um cliente cadastrado com este CPF/CNPJ!");
            //            else
            //            {
            //                ClienteModel.Add(Entity);
            //            }
            //        }
            //        else
            //        {
            //            ClienteModel.Update(Entity);
            //        }
            //        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            //}
            return Page();
        }

        #region Outros Métodos
        private void CarregarLists()
        {
            ListUsuario = UsuarioModel.GetList();
            ListKPIMetaValor = KPIMetaValorModel.GetList();
            ListKPIMetaVida = KPIMetaVidaModel.GetList();
        }
        #endregion
        
        #endregion
    }
}
