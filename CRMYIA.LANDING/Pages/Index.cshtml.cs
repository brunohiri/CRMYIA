using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CRMYIA.Landing.Pages
{
    public class IndexModel : PageModel
    {    
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public LandingPage Entity { get; set; }
        [BindProperty]
        public Usuario Corretor { get; set; }
        #endregion

        #region Construtores
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            //?id=S%252bMVSor2q%252bNSABf%252f6fwRng%253d%253d
            if (Id.IsNullOrEmpty())
            {
                Usuario usuario = new Usuario();
                Corretor = usuario;
            }
            else
            {
               var idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();
                Corretor = UsuarioModel.Get(idUser);
            }
            return Page();
        }

        public IActionResult OnPost(string Id = null)
        {
            try
            {
              var a =  Request.Form;
                var IpCliente = HttpContext.Connection.RemoteIpAddress.ToString();
                var idUser = Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong();

                if (Entity.IdLandingPage == 0)
                {
                    if (Entity.Nome.Length > 0)
                    {
                        Entity.DataCadastro = DateTime.Now;
                        Entity.IP = IpCliente;
                        Entity.IdUsuario = idUser;
                        LandingPageModel.Add(Entity);
                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    }
                    else
                    {
                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Verifique os campos!");
                    }
                }
                else
                {
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Não foi possivel salvar!");
                }

            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Erro, ex.Message);
            }
            return Page();
        }

        #endregion
    }
}
