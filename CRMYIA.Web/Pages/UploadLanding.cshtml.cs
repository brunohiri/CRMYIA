using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
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
    public class UploadLanding : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivo { get; set; }
        [BindProperty]
        public LandingPageCarrossel Entity { get; set; }
        [BindProperty]
        public List<Usuario> ListCorretor { get; set; }
        [BindProperty]
        public List<LandingPageCarrossel> ListEntity { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        #endregion

        #region Construtores
        public UploadLanding(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
            {
                Entity = new LandingPageCarrossel();
                Entity.DataCadastro = DateTime.Now;
            }
            else
            {
                Entity = LandingPageCarrosselModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ImagemDiferente = Entity.NomeArquivo;
            }
            if (Entity.CaminhoArquivo != null && Entity.NomeArquivo != null)
            {
                CaminhoImagem = Entity.CaminhoArquivo + Entity.NomeArquivo;
            }
            else
            {
                CaminhoImagem = "/img/carrosselLanding/imgCorretor.png";
            }
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadLandingPageAsync(LandingPageCarrossel formData)//long IdCampanhaArquivo, long IdCampanha, string Descricao, string Observacao, bool Ativo)
        {
            try
            {
                var documentFile = Request.Form.Files.ToList();
                List<LandingPageCarrossel> EntityLista = null;

                LandingPageCarrossel EntityLanding = null;

                MensagemModel mensagem = null;
                List<string> FilesNomes = new List<string>();
                bool status = false;

                foreach (IFormFile Item in documentFile)
                {
                    FilesNomes.Add(Item.FileName);
                }

                if (formData.Titulo == null && documentFile == null)
                {
                    mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Por favor verifique o formulário.");
                }
                else
                {
                    if (mensagem == null)
                    {
                        var msg = "";
                        if (formData.IdLandingPageCarrossel == 0)
                        {
                            msg = "Carregado";
                            int i = 0;
                            string NomeArquivo = string.Empty;
                            string Titulo = string.Empty;
                            string[] VetFileName;

                            foreach (var Item in documentFile)
                            {
                                string NomeArquivoOriginal = Item.FileName;

                                NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCarrosselLanding"], NomeArquivo);

                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await Item.CopyToAsync(fileStream);
                                    CaminhoImagem = "ArquivoCarrosselLanding/";
                                    VetFileName = Item.FileName.Split('-');
                                }

                                LandingPageCarrosselModel.Add(new LandingPageCarrossel()
                                {
                                    IdLandingPageCarrossel = formData.IdLandingPageCarrossel,
                                    IdUsuario = formData.IdUsuario,
                                    Titulo = formData.Titulo,
                                    CaminhoArquivo = "ArquivoCarrosselLanding/",
                                    NomeArquivo = NomeArquivo,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });
                            }
                            EntityLista = LandingPageCarrosselModel.GetList();
                            status = true;
                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                        }
                        else
                        {
                            //UpDate Texto Informacao
                            EntityLanding = LandingPageCarrosselModel.Get(formData.IdLandingPageCarrossel);

                            if (EntityLanding != null)
                            {
                                LandingPageCarrosselModel.Update(new LandingPageCarrossel()
                                {
                                    IdLandingPageCarrossel = formData.IdLandingPageCarrossel,
                                    IdUsuario = formData.IdUsuario,
                                    Titulo = formData.Titulo,
                                    CaminhoArquivo = EntityLanding.CaminhoArquivo,
                                    NomeArquivo = EntityLanding.NomeArquivo,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });

                            }

                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Registro atualizado com Sucesso!!!");
                            EntityLista = LandingPageCarrosselModel.GetList();
                            status = true;
                            return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
                        }
                    }
                }
                return new JsonResult(new { entityLista = EntityLista, mensagem = mensagem, status = status });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAlterarImagemAsync()
        {
            try
            {
                List<LandingPageCarrossel> EntityLista = null;
                LandingPageCarrossel Entity = null;
                bool status = false;
                MensagemModel mensagem = null;

                var file = Request.Form.Files.FirstOrDefault();
                string IdLandingCarrossel = Request.Form["IdLandingPageCarrossel"].ToString();
                string NomeArquivo = Request.Form["NomeArquivo"].ToString();
                Entity = LandingPageCarrosselModel.Get(IdLandingCarrossel.ExtractLong());
                string msg = "Alterado";
                string NovoNomeArquivo = string.Empty;

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                {
                    string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCarrosselLanding"], NomeArquivo);
                    if ((System.IO.File.Exists(_imageToBeDeleted)))
                    {
                        System.IO.File.Delete(_imageToBeDeleted);
                    }
                    string NomeArquivoOriginal = file.FileName;

                    NovoNomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCarrosselLanding"], NovoNomeArquivo);

                    Entity.NomeArquivo = "";

                    using (var fileStream = new FileStream(arquivo, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        Entity.CaminhoArquivo = "ArquivoCarrosselLanding/";
                    }
                }
                else
                {
                    Entity.CaminhoArquivo = "img/carrosselLanding/";
                    Entity.NomeArquivo = "imgCarrossel.png";
                }
                LandingPageCarrosselModel.Update(new LandingPageCarrossel()
                {
                    IdLandingPageCarrossel = Entity.IdLandingPageCarrossel,
                    IdUsuario = Entity.IdUsuario,
                    Titulo = Entity.Titulo,
                    CaminhoArquivo = Entity.CaminhoArquivo,
                    NomeArquivo = NovoNomeArquivo,
                    DataCadastro = DateTime.Now,
                    Ativo = Entity.Ativo
                });

                EntityLista = LandingPageCarrosselModel.GetList();
                status = true;
                mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterado com sucesso!");
                
                return new JsonResult(new { entityLista = EntityLista, mensagem = mensagem, status = status });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public IActionResult OnPostExcluirImagem(string idlandcarrossel)
        {
            try
            {
                List<LandingPageCarrossel> EntityLista = null;
                LandingPageCarrossel Entity = null;
                bool status = false;
                string Mensagem = "";
                Entity = LandingPageCarrosselModel.Get(idlandcarrossel.ExtractLong());

                Entity.Ativo = false;
                LandingPageCarrosselModel.Update(Entity);
                EntityLista = LandingPageCarrosselModel.GetList();
                status = true;
                Mensagem = "Imagem Excluída com Sucesso!";

                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = Mensagem });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }
        public IActionResult OnPostObter(string IdLandingCarrossel)
        {

            LandingPageCarrossel EntityLandingCarrossel = null;
            bool status = false;
            Mensagem = null;
            try
            {
                EntityLandingCarrossel = LandingPageCarrosselModel.Get(IdLandingCarrossel.ExtractLong());
                status = true;
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, ex.Message);
                return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityLandingCarrossel });
            }
            return new JsonResult(new { status = status, entityLista = EntityLandingCarrossel });
        }

        public void CarregarLists()
        {
            ListCorretor = UsuarioModel.GetList();
            ListEntity = LandingPageCarrosselModel.GetList();
        }
        #endregion
    }
}
