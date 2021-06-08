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
    public class NovoVideo : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }
        [BindProperty]
        public CampanhaArquivo Entity { get; set; }

        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }
        [BindProperty]
        public List<Calendario> ListCalendario { get; set; }
        [BindProperty]
        public List<VideoViewModel> ListEntity { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        #endregion

        #region Construtores
        public NovoVideo(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            //if (Id.IsNullOrEmpty())
            //{
            //    Entity = new CampanhaArquivo();
            //    Entity.DataCadastro = DateTime.Now;
            //}
            //else
            //{
            //    Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            //    ImagemDiferente = Entity.NomeArquivo;
            //}
            //if (Entity.CaminhoArquivo != null && Entity.NomeArquivo != null)
            //{
            //    CaminhoImagem = Entity.CaminhoArquivo + Entity.NomeArquivo;
            //}
            //else
            //{
            //    CaminhoImagem = "/img/fotoCadastro/foto-cadastro.jpeg";
            //}
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadVideoAsync([FromForm] List<ICollection<IFormFile>> files, Video formData) //long IdCampanhaArquivo, long IdCampanha, string Descricao, string Observacao, bool Ativo)
        {
            try
            {
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                var documentFile = Request.Form.Files.ToList();
                List<VideoViewModel> EntityLista = null;
                MensagemModel mensagem = null;

                bool status = false;

                if (formData.IdentificadorVideo == null && files == null)
                {
                    mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Por favor verifique o formulário.");
                }
                else
                {
                    if (mensagem == null)
                    {
                        var msg = "";
                        if (formData.IdVideo == 0)
                        {
                            msg = "Carregado";
                            int i = 0;
                            string NomeArquivo = string.Empty;
                            foreach (var Item in documentFile)
                            {
                                string NomeArquivoOriginal = Item.FileName;

                                NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoVideo"], NomeArquivo);

                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await Item.CopyToAsync(fileStream);
                                    formData.CaminhoArquivo = "ArquivoVideo/";
                                    CaminhoImagem = "ArquivoVideo/";
                                }
                                i++;
                            }

                            if(formData.IdCalendario == 0)
                            {
                                VideoModel.Add(new Video()
                                {
                                    IdUsuario = IdUsuario,
                                    IdCampanha = formData.IdCampanha,
                                    IdentificadorVideo = formData.IdentificadorVideo,
                                    IdCalendario = null,
                                    CaminhoArquivo = "ArquivoVideo/",
                                    NomeVideo = NomeArquivo,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });
                            }
                            else
                            {
                                VideoModel.Add(new Video()
                                {
                                    IdUsuario = IdUsuario,
                                    IdCampanha = formData.IdCampanha,
                                    IdentificadorVideo = formData.IdentificadorVideo,
                                    IdCalendario = formData.IdCalendario,
                                    CaminhoArquivo = "ArquivoVideo/",
                                    NomeVideo = NomeArquivo,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });
                            }
                            
                            EntityLista = VideoModel.GetList();
                            status = true;
                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                        }
                    }
                }
                return new JsonResult(new
                {
                    entityLista = EntityLista,
                    mensagem = mensagem,
                    status = status
                });

            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
           
        }

        public async Task<IActionResult> OnPostAlterarVideoAsync()
        {
            try
            {
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                List<VideoViewModel> EntityLista = null;
                bool status = false;
                MensagemModel mensagem = null;

                var file = Request.Form.Files.FirstOrDefault();
                string IdVideo = Request.Form["IdVideo"].ToString();
                string NomeVideo = Request.Form["NomeVideo"].ToString();
                string IdentificadorVideo = Request.Form["ModalIdVideo"].ToString();
                string IdCalendario = Request.Form["IdCalendario"].ToString();
                Video Entity = null;
                Entity = VideoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdVideo)).ExtractLong());
                string msg = "Alterado";
                //string NomeArquivo = string.Empty;

                if (!string.IsNullOrEmpty(NomeVideo) && file != null)
                {
                    string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoVideo"], NomeVideo);
                    if ((System.IO.File.Exists(_imageToBeDeleted)))
                    {
                        System.IO.File.Delete(_imageToBeDeleted);
                    }
                    string NomeArquivoOriginal = file.FileName;

                    string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                    var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoVideo"], NovoNomeArquivo);
                    using (var fileStream = new FileStream(arquivo, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        Entity.CaminhoArquivo = "ArquivoVideo/";
                        Entity.NomeVideo = NovoNomeArquivo;
                        Entity.DataCadastro = DateTime.Now;
                    }

                    VideoModel.Update(new Video()
                        {
                            IdVideo = Entity.IdVideo,
                            IdUsuario = IdUsuario,
                            IdCampanha = Entity.IdCampanha,
                            IdentificadorVideo = IdentificadorVideo,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeVideo = Entity.NomeVideo,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                     });
                    
                    EntityLista = VideoModel.GetList();
                    status = true;
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterado com sucesso!");
                }

                return new JsonResult(new
                {
                    entityLista = EntityLista,
                    mensagem = mensagem,
                    status = status
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public IActionResult OnPostExcluirVideo([FromBody] VideoViewModel obj)
        {
            try
            {
                List<VideoViewModel> EntityLista = null;
                Video Entity = null;
                bool status = false;
                string Mensagem = "";
                if (obj.IdVideo.ToString() != null)
                {
                    Entity = VideoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(obj.IdVideo)).ExtractLong());

                    Entity.Ativo = false;
                    VideoModel.Update(Entity);
                    EntityLista = VideoModel.GetList();
                    status = true;
                    Mensagem = "Vídeo Excluído com Sucesso!";
                }
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = Mensagem });
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
        }

        public IActionResult OnPostEditarFormulario([FromBody] VideoViewModel obj)
        {
            try
            {
                Video Entity = null;
                bool status = false;
                if (obj.IdVideo.ToString() != null)
                {
                    Entity = VideoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(obj.IdVideo)).ExtractLong());
                   
                    VideoModel.Update(Entity);
                    status = true;
                }
                return new JsonResult(new { 
                        status = status,
                        entity = new VideoViewModel{
                                                        
                                                        IdCampanha = Entity.IdCampanha.ToString().ExtractLong(),
                                                        IdVideo = HttpUtility.UrlEncode(Criptography.Encrypt(Entity.IdVideo.ToString()).ToString()),
                                                        IdentificadorVideo = Entity.IdentificadorVideo,
                                                        Ativo = Entity.Ativo,
                                                        IdCalendario = Entity.IdCalendario
                        } 
                    });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }

        public IActionResult OnPostSalvarAlteracaoVideo([FromBody] VideoViewModel obj)
        {
            try
            {
                List<VideoViewModel> EntityLista = null;
                Video Entity = null;
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                bool status = false;
                Mensagem = null;
                if (obj.IdVideo.ToString() != null)
                {
                    Entity = VideoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(obj.IdVideo)).ExtractLong());

                    if(obj.IdCalendario == 0)
                    {
                        VideoModel.Update(new Video()
                        {
                            IdVideo = Criptography.Decrypt(HttpUtility.UrlDecode(obj.IdVideo)).ExtractLong(),
                            IdUsuario = IdUsuario,
                            IdCampanha = obj.IdCampanha,
                            IdCalendario = null,
                            IdentificadorVideo = obj.IdentificadorVideo,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeVideo = Entity.NomeVideo,
                            DataCadastro = DateTime.Now,
                            Ativo = obj.Ativo
                        });
                    }
                    else
                    {
                        VideoModel.Update(new Video()
                        {
                            IdVideo = Criptography.Decrypt(HttpUtility.UrlDecode(obj.IdVideo)).ExtractLong(),
                            IdUsuario = IdUsuario,
                            IdCampanha = obj.IdCampanha,
                            IdCalendario = obj.IdCalendario,
                            IdentificadorVideo = obj.IdentificadorVideo,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeVideo = Entity.NomeVideo,
                            DataCadastro = DateTime.Now,
                            Ativo = obj.Ativo
                        });
                    }
                   
                    EntityLista = VideoModel.GetList();
                    status = true;
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Vídeo Atualizado com Sucesso!");
                }
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = Mensagem });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public void CarregarLists()
        {
            ListEntity = VideoModel.GetList();
            ListCalendario = Business.CalendarioModel.GetList();
            ListCampanha = Business.CampanhaModel.GetList();
        }
        #endregion
    }
}
