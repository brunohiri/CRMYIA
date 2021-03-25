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
    public class UploadBannerOperadora : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }
        [BindProperty]
        public Banner Entity { get; set; }
        [BindProperty]
        public RedeSocial EntityRedeSocial { get; set; }

        [BindProperty]
        public List<CapaRedeSocial> ListCapaRedeSocial { get; set; }

        [BindProperty]
        public List<CapaRedeSocialViewModel> ListEntity { get; set; }
        [BindProperty]
        public List<BannerOperadoraViewModel> ListBannerOperadora { get; set; }
        [BindProperty]
        public List<Operadora> ListOperadora { get; set; }
        public long? IdOperadora { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        #endregion

        #region Construtores
        public UploadBannerOperadora(IConfiguration configuration, IHostingEnvironment environment)
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
                Entity = new Banner();
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

        public async Task<IActionResult> OnPostUploadBannerOperadoraAsync(EnviarBannerOperadoraViewModel formData)
        {
            try
            {
                long IdBanner =  formData.IdBanner.ExtractLong();
                string Descricao = formData.Descricao;
                bool Ativo = formData.Ativo;
                long IdOperadora = formData.IdOperadora.ExtractLong();
                long IdInformacao = formData.IdInformacao.ExtractLong();
                var documentFile = Request.Form.Files.ToList();
                List<BannerOperadoraViewModel> EntityLista = null;
                MensagemModel mensagem = null;
                Banner EntityBanner = null;
                BannerOperadora EntityBannerOperadora = null;
                Informacao EntityInformacao = null;
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                List<string> FilesNomes = new List<string>();
                bool status = false;
                foreach (IFormFile Item in documentFile)
                {
                    FilesNomes.Add(Item.FileName);
                }
                if (!Util.VerificaNomeArquivo(FilesNomes))
                {
                    mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Nome do arquivo não esta no padrão!");
                }
                else
                {
                    if (mensagem == null)
                    {
                        var msg = "";
                        if (IdBanner == 0 && documentFile != null)
                        {
                            msg = "Carregado";
                            int i = 0;
                            int Width = 0;
                            int Height = 0;
                            string NomeArquivo = string.Empty;
                            InformacaoModel.Add(new Informacao
                            {
                                Descricao = Descricao,
                                DataCadastro = DateTime.Now,
                                Ativo = formData.Ativo
                            });
                            foreach (var Item in documentFile)
                            {
                                
                                string NomeArquivoOriginal = Item.FileName;

                                NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoBannerOperadora"], NomeArquivo);

                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await Item.CopyToAsync(fileStream);
                                }
                                using (var image = Image.FromStream(Item.OpenReadStream()))
                                {
                                    Width = image.Width;
                                    Height = image.Height;
                                }
                                //Grava um registro Banner
                                EntityInformacao = InformacaoModel.GetLastId();
                                var retorno = BannerOperadoraModel.AddBool(new Banner()
                                {
                                    IdInformacao = EntityInformacao.IdInformacao,
                                    CaminhoArquivo = "ArquivoBannerOperadora/",
                                    NomeArquivo = NomeArquivo,
                                    Width = Width,
                                    Height = Height,
                                    DataCadastro = DateTime.Parse(DateTime.Now.ToString()),
                                    Ativo = Ativo
                                });

                                EntityBanner = BannerOperadoraModel.GetLastId();

                                if (retorno && EntityBanner != null)
                                {
                                    BannerOperadoraModel.Add(new BannerOperadora()
                                    {
                                        IdUsuario = IdUsuario,
                                        IdOperadora = IdOperadora,
                                        IdBanner = EntityBanner.IdBanner
                                    });
                                }
                            }
                            EntityLista = BannerOperadoraModel.GetList();
                            status = true;
                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                        }
                        else
                        {
                            //Update Texto
                            EntityBanner = BannerOperadoraModel.Get(IdBanner);
                            EntityBannerOperadora = BannerOperadoraModel.Get(EntityBanner.IdBanner.ToString());
                            if (EntityBanner != null && EntityBannerOperadora != null)
                            {
                                InformacaoModel.Update(new Informacao()
                                {
                                    IdInformacao = IdInformacao,
                                    Descricao = Descricao,
                                    DataCadastro = DateTime.Now,
                                    Ativo = Ativo
                                });

                                BannerOperadoraModel.Update(new Banner()
                                {
                                    IdBanner = IdBanner,
                                    CaminhoArquivo = EntityBanner.CaminhoArquivo,
                                    NomeArquivo = EntityBanner.NomeArquivo,
                                    Width = EntityBanner.Width,
                                    Height = EntityBanner.Height,
                                    DataCadastro = DateTime.Now,
                                    Ativo = Ativo
                                });
                            
                                BannerOperadoraModel.Update(new BannerOperadora()
                                {
                                    IdBannerOperadora = EntityBannerOperadora.IdBannerOperadora,
                                    IdUsuario = IdUsuario,
                                    IdOperadora = IdOperadora,
                                    IdBanner = EntityBanner.IdBanner
                                });
                            }

                            mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Registro atualizado com Sucesso!!!");
                            EntityLista = BannerOperadoraModel.GetList();
                            status = true;
                            return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
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
                List<BannerOperadoraViewModel> EntityLista = null;
                bool status = false;
                MensagemModel mensagem = null;
                var file = Request.Form.Files.FirstOrDefault();
                string IdBanner = Request.Form["IdBanner"].ToString();
                string NomeArquivo = Request.Form["NomeArquivo"].ToString();
                Banner Entity = null;
                Entity = BannerOperadoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdBanner)).ExtractLong());
                string msg = "Alterado";

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (Util.VerificaNomeArquivo(FilesNomes))
                {

                    if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                    {
                        string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoBannerOperadora"], NomeArquivo);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        string NomeArquivoOriginal = file.FileName;

                        string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                        var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoBannerOperadora"], NovoNomeArquivo);

                        using (var fileStream = new FileStream(arquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Entity.CaminhoArquivo = "ArquivoBannerOperadora/";
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
                       BannerOperadoraModel.Update(new Banner()
                        {
                           IdBanner = Entity.IdBanner,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeArquivo = Entity.NomeArquivo,
                            Width = Width,
                            Height = Height,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                        });
                    }

                    EntityLista = BannerOperadoraModel.GetList();
                    status = true;
                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterado com sucesso!");
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

        //public IActionResult OnPostAlterarTexto(EnviarBannerOperadoraViewModel formData)
        //{
        //    bool status = false;
        //    List<BannerOperadoraViewModel> EntityBanner = null;
        //    try
        //    {


               
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public IActionResult OnPostExcluirImagem(IFormCollection dados)
        {
            List<BannerOperadoraViewModel> EntityLista = null;
            bool status = false;
            string mensagem = "Erro ao Excluir Imagem";
            long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
            try
            {
                Banner EntityBannerOperadora = null;
                EntityBannerOperadora = BannerOperadoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(dados["IdBanner"])).ExtractLong());
                BannerOperadoraModel.Update(new Banner()
                {
                    IdBanner = EntityBannerOperadora.IdBanner,
                    CaminhoArquivo = EntityBannerOperadora.CaminhoArquivo,
                    NomeArquivo = EntityBannerOperadora.NomeArquivo,
                    Width = EntityBannerOperadora.Width,
                    Height = EntityBannerOperadora.Height,
                    DataCadastro = EntityBannerOperadora.DataCadastro,
                    Ativo = false
                });
                EntityLista = BannerOperadoraModel.GetList();
                status = true;
                mensagem = "Imagem Excluída com Sucesso!";
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
            }
            return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
        }

        public IActionResult OnPostObter(string IdBanner)
        {
            Banner EntityBanner = null;
            BannerOperadora EntityBannerOperadora = null;
            Informacao EntityInformacao = null;
            bool status = false;
            Mensagem = null;
            try
            {
                EntityBanner = BannerOperadoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdBanner)).ExtractLong());
                EntityBannerOperadora = BannerOperadoraModel.Get(EntityBanner.IdBanner.ToString());
                EntityInformacao = InformacaoModel.Get(EntityBanner.IdInformacao.ToString().ExtractLong());
                
                status = true;
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, ex.Message);
                return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityBanner });
            }
            return new JsonResult(new { status = status, entityLista = EntityBanner, idOperadora = EntityBannerOperadora.IdOperadora, entityInformacao = EntityInformacao});
        }
        public void CarregarLists()
        {
            ListOperadora = OperadoraModel.GetListIdDescricao();
            ListBannerOperadora = BannerOperadoraModel.GetList();
        }
        #endregion
    }
}
