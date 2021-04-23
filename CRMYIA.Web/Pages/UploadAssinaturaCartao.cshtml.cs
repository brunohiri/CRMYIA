using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
    public class UploadAssinaturaCartao : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }
        [BindProperty]
        public AssinaturaCartao Entity { get; set; }
        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }
        
        [BindProperty]
        public List<AssinaturaCartaoViewModel> ListAssinaturaCartao { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        #endregion

        #region Construtores
        public UploadAssinaturaCartao(IConfiguration configuration, IHostingEnvironment environment)
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
                Entity = new AssinaturaCartao();
                Entity.DataCadastro = DateTime.Now;
            }
            //else
            //{
            //    //Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
            //    ImagemDiferente = Entity.NomeArquivo;
            //}
            
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAssinaturaCartaoAsync(IFormCollection dados)
        {
            try
            {
                long IdAssinaturaCartao =  dados["IdAssinaturaCartao"].ToString().ExtractLong();
                string Titulo = dados["Titulo"];
                bool Ativo = Convert.ToBoolean(dados["Ativo"].Contains("true"));
                string IdCampanha = dados["IdCampanha"].ToString();
                var documentFile = Request.Form.Files.ToList();
                AssinaturaCartao Entity = null;
                List<AssinaturaCartaoViewModel> EntityLista = null;
                MensagemModel mensagem = null;
                List<string> FilesNomes = new List<string>();
                bool status = false;
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                //if (CampanhaArquivoModel.GetCampanhaId(formData.IdCampanha.ToString().ExtractLong()))

                foreach (IFormFile Item in documentFile)
                {
                    FilesNomes.Add(Item.FileName);
                }
                if (!Util.VerificaNomeArquivoAssinaturaCartao(FilesNomes))
                {
                    mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Nome do arquivo não esta no padrão!");
                }
                else
                {
                    if (mensagem == null)
                    {
                        var msg = "";
                        if (IdAssinaturaCartao == 0 && documentFile != null)
                        {
                            msg = "Carregado";
                            int i = 0;
                            int Width = 0;
                            int Height = 0;
                            string NomeArquivo = string.Empty;
                            
                            foreach (var Item in documentFile)
                            {
                                string NomeArquivoOriginal = Item.FileName;

                                NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoAssinaturaCartao"], NomeArquivo);

                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await Item.CopyToAsync(fileStream);
                                }
                                using (var image = Image.FromStream(Item.OpenReadStream()))
                                {
                                    Width = image.Width;
                                    Height = image.Height;
                                }
                                //Grava um registro Capa
                                AssinaturaCartaoModel.Add(new AssinaturaCartao()
                                {
                                    IdUsuario = IdUsuario,
                                    IdCampanha = IdCampanha.ExtractLong(),
                                    Titulo = Titulo,
                                    CaminhoArquivo = "ArquivoAssinaturaCartao/",
                                    NomeArquivo = NomeArquivo,
                                    Width = Width,
                                    Height = Height,
                                    DataCadastro = DateTime.Parse(DateTime.Now.ToString()),
                                    Ativo = Ativo
                                });
                            }
                            EntityLista = AssinaturaCartaoModel.GetList();
                            status = true;
                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                        }
                        else
                        {
                            Entity = AssinaturaCartaoModel.Get(IdAssinaturaCartao);
                           if(Entity != null)
                            {
                                AssinaturaCartaoModel.Update(new AssinaturaCartao()
                                {
                                    IdAssinaturaCartao = IdAssinaturaCartao,
                                    IdUsuario = IdUsuario,
                                    IdCampanha = IdCampanha.ExtractLong(),
                                    Titulo = Titulo,
                                    CaminhoArquivo = Entity.CaminhoArquivo,
                                    NomeArquivo = Entity.NomeArquivo,
                                    Width = Entity.Width,
                                    Height = Entity.Height,
                                    DataCadastro = DateTime.Now,
                                    Ativo = Ativo
                                });
                                EntityLista = AssinaturaCartaoModel.GetList();
                                status = true;
                                mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterado com sucesso!");
                            }
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
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }

        public async Task<IActionResult> OnPostAlterarImagemAsync()
        {
            try
            {
                List<AssinaturaCartaoViewModel> EntityLista = null;
                bool status = false;
                MensagemModel mensagem = null;
                var file = Request.Form.Files.FirstOrDefault();
                string IdAssinaturaCartao = Request.Form["IdAssinaturaCartao"].ToString();
                string NomeArquivo = Request.Form["NomeArquivo"].ToString();
                AssinaturaCartao Entity = null;
                Entity = AssinaturaCartaoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdAssinaturaCartao)).ExtractLong());
                string msg = "Alterado";

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (Util.VerificaNomeArquivo(FilesNomes))
                {

                    if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                    {
                        string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoAssinaturaCartao"], NomeArquivo);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        string NomeArquivoOriginal = file.FileName;

                        string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                        var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoAssinaturaCartao"], NovoNomeArquivo);

                        using (var fileStream = new FileStream(arquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Entity.CaminhoArquivo = "ArquivoAssinaturaCartao/";
                            Entity.NomeArquivo = NovoNomeArquivo;
                        }
                        int Width = 0;
                        int Height = 0;
                        using (var image = Image.FromStream(file.OpenReadStream()))
                        {
                            Width = image.Width;
                            Height = image.Height;
                        }
                        long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                        AssinaturaCartaoModel.Update(new AssinaturaCartao()
                        {
                            IdUsuario = IdUsuario,
                            IdAssinaturaCartao = Entity.IdAssinaturaCartao,
                            Titulo = Entity.Titulo,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeArquivo = Entity.NomeArquivo,
                            Width = Width,
                            Height = Height,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                        });
                    }

                    EntityLista = AssinaturaCartaoModel.GetList();
                    status = true;
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Imagem alterado com sucesso!");
                }
                else
                {
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Selecione uma Imagem");
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

        public IActionResult OnPostExcluirImagem(IFormCollection dados)
        {
            List<AssinaturaCartaoViewModel> EntityLista = null;
            bool status = false;
            string mensagem = "Erro ao Excluir Imagem";
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            try
            {
                AssinaturaCartao EntityAssinaturaCartao = null;
                EntityAssinaturaCartao = AssinaturaCartaoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(dados["IdAssinaturaCartao"])).ExtractLong());
                AssinaturaCartaoModel.Update(new AssinaturaCartao()
                {
                    IdUsuario = IdUsuario,
                    IdAssinaturaCartao = EntityAssinaturaCartao.IdAssinaturaCartao,
                    Titulo = EntityAssinaturaCartao.Titulo,
                    CaminhoArquivo = EntityAssinaturaCartao.CaminhoArquivo,
                    NomeArquivo = EntityAssinaturaCartao.NomeArquivo,
                    Width = EntityAssinaturaCartao.Width,
                    Height = EntityAssinaturaCartao.Height,
                    DataCadastro = EntityAssinaturaCartao.DataCadastro,
                    Ativo = false
                });
                EntityLista = AssinaturaCartaoModel.GetList();
                status = true;
                mensagem = "Imagem Excluída com Sucesso!";
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
            }
            return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
        }

        public IActionResult OnPostObter(string IdAssinaturaCartao)
        {
            AssinaturaCartao EntityAssinaturaCartao = null;
            bool status = false;
            try
            {
                EntityAssinaturaCartao = AssinaturaCartaoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdAssinaturaCartao)).ExtractLong());
               
                status = true;
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, ex.Message);
                return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityAssinaturaCartao });
            }
            return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityAssinaturaCartao });
        }
        public void CarregarLists()
        {
            ListAssinaturaCartao = AssinaturaCartaoModel.GetList();
            ListCampanha = Business.CampanhaModel.GetList();
        }
        #endregion
    }
}
