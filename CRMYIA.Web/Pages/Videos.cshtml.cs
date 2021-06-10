using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    [Authorize]
    public class Videos : PageModel
    {
        #region Propriedades
        readonly IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        [BindProperty]
        public List<Perfil> ListEntity { get; set; }
        #endregion

        #region Construtores
        public Videos(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Métodos
        //public IActionResult OnGet()
        //{
           
        //    return Page();
        //}

        public IActionResult OnPostListarVideos()
        {
            //string Id = Request.Form["Id"].ToString();
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            Usuario EntityUsuario = null;
            EntityUsuario = UsuarioModel.Get(IdUsuario);

            bool status = false;
            List<Video> EntityVideo = VideoModel.GetListaVideos(/*Criptography.Decrypt(Id).ExtractLong(),*/ (byte)EntityUsuario.IdGrupoCorretor);
            if (EntityVideo.Count > 0)
            {
                status = true;
            }
            return new JsonResult(new { status = status, entityVideo = EntityVideo });
        }

        public IActionResult OnPostContadorDownload(Campanha fromData)
        {
            bool status = false;
            try
            {
                if (fromData.IdCampanha > 0 && fromData.IdCampanha.ToString() != null)
                {
                    Campanha EntityCampanha = CampanhaModel.Get(fromData.IdCampanha);
                    EntityCampanha.QuantidadeDownload = (EntityCampanha.QuantidadeDownload + 1);
                    CampanhaModel.Update(EntityCampanha);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult(new
            {
                status = status
            });
        }
        #endregion
    }
}
