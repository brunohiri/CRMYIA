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
    public class UploadCapaRedeSocial : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoCampanha { get; set; }
        [BindProperty]
        public RedeSocial Entity { get; set; }
        [BindProperty]
        public RedeSocial EntityRedeSocial { get; set; }

        [BindProperty]
        public List<CapaRedeSocial> ListCapaRedeSocial { get; set; }
        [BindProperty]
        public List<Campanha> ListCampanha { get; set; }

        [BindProperty]
        public List<CapaRedeSocialViewModel> ListEntity { get; set; }
        [BindProperty]
        public List<RedeSocial> ListRedeSocial { get; set; }
        [BindProperty]
        public List<CapaViewModel> ListCapa { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        [BindProperty]
        public List<Calendario> ListCalendario { get; set; }
        #endregion

        #region Construtores
        public UploadCapaRedeSocial(IConfiguration configuration, IHostingEnvironment environment)
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
                Entity = new RedeSocial();
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

        public async Task<IActionResult> OnPostUploadCapaRedeSocialAsync(/*[FromForm] List<ICollection<IFormFile>> files,*/ EnviarCapaRedeSocialViewModel formData)
        {
            try
            {
                var documentFile = Request.Form.Files.ToList();
                List<CapaViewModel> EntityLista = null;
                CapaRedeSocial EntityCapaRedeSocial = null;
                Capa EntityCapa = null;
                MensagemModel mensagem = null;
                List<string> FilesNomes = new List<string>();
                bool status = false;
                long IdUsuario = HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value.ExtractLong();
                //if (CampanhaArquivoModel.GetCampanhaId(formData.IdCampanha.ToString().ExtractLong()))

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
                        if (formData.IdCapa == 0 && documentFile != null)
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
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCapaRedeSocial"], NomeArquivo);

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
                                var retorno = false;
                                if (formData.IdCalendario == 0)
                                {
                                     retorno = CapaModel.Add(new Capa()
                                    {
                                        IdCalendario = null,
                                        Titulo = formData.Titulo,
                                        CaminhoArquivo = "ArquivoCapaRedeSocial/",
                                        NomeArquivo = NomeArquivo,
                                        Width = Width,
                                        Height = Height,
                                        DataCadastro = DateTime.Parse(DateTime.Now.ToString()),
                                        Ativo = formData.Ativo
                                    });
                                }
                                else
                                {
                                     retorno = CapaModel.Add(new Capa()
                                    {
                                        IdCalendario = formData.IdCalendario,
                                        Titulo = formData.Titulo,
                                        CaminhoArquivo = "ArquivoCapaRedeSocial/",
                                        NomeArquivo = NomeArquivo,
                                        Width = Width,
                                        Height = Height,
                                        DataCadastro = DateTime.Parse(DateTime.Now.ToString()),
                                        Ativo = formData.Ativo
                                    });
                                }

                                //Pega o ultimo valor
                                EntityCapa = CapaModel.GetLastId();

                                //Grava um registro CapaRedeSocial
                                if (EntityCapa != null && retorno)
                                {
                                    CapaRedeSocialModel.Add(new CapaRedeSocial()
                                    {
                                        IdRedeSocial = formData.IdRedeSocial,
                                        IdCapa = EntityCapa.IdCapa,
                                        IdUsuario = IdUsuario,
                                        IdCampanha = formData.IdCampanha
                                    });

                                    EntityLista = CapaModel.GetList();
                                    status = true;
                                    mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                                }
                            }
                        }
                        else
                        {
                            //Update Texto
                            EntityCapaRedeSocial = CapaRedeSocialModel.Get(formData.IdCapa);
                            EntityCapa = CapaModel.Get(formData.IdCapa);
                            if(EntityCapaRedeSocial != null && EntityCapa != null) {
                                CapaRedeSocialModel.Update(new CapaRedeSocial()
                                {
                                    IdCapaRedeSocial = EntityCapaRedeSocial.IdCapaRedeSocial,
                                    IdRedeSocial = formData.IdRedeSocial,
                                    IdCapa = formData.IdCapa,
                                    IdUsuario = IdUsuario,
                                    IdCampanha = formData.IdCampanha
                                });

                                CapaModel.Update(new Capa()
                                {
                                    IdCapa = formData.IdCapa,
                                    IdCalendario = formData.IdCalendario,
                                    Titulo = formData.Titulo,
                                    CaminhoArquivo = EntityCapa.CaminhoArquivo,
                                    NomeArquivo = EntityCapa.NomeArquivo,
                                    Width = EntityCapa.Width,
                                    Height = EntityCapa.Height,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });
                                EntityLista = CapaModel.GetList();
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

        public IActionResult OnPostSalvarRedeSocialAsync(string NomeRedeSocial, bool Ativo)
        {
            bool status = false;
            List<RedeSocial> EntityRetorno = null;
            try
            {
                if (NomeRedeSocial != null)
                {
                    RedeSocialModel.Add(new RedeSocial()
                    { 
                        Nome = NomeRedeSocial, 
                        DataCadastro = DateTime.Now, 
                        Ativo = Ativo
                    });
                    EntityRetorno = RedeSocialModel.GetList();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult(new { status = status, EntityRetorno = EntityRetorno });
        }

        public async Task<IActionResult> OnPostAlterarImagemAsync()
        {
            try
            {
                List<CapaViewModel> EntityLista = null;
                bool status = false;
                MensagemModel mensagem = null;
                var file = Request.Form.Files.FirstOrDefault();
                string IdCapa = Request.Form["IdCapa"].ToString();
                string NomeArquivo = Request.Form["NomeArquivo"].ToString();
                Capa Entity = null;
                Entity = CapaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCapa)).ExtractLong());
                string msg = "Alterado";

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (Util.VerificaNomeArquivo(FilesNomes))
                {

                    if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                    {
                        string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCapaRedeSocial"], NomeArquivo);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        string NomeArquivoOriginal = file.FileName;

                        string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                        var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCapaRedeSocial"], NovoNomeArquivo);

                        using (var fileStream = new FileStream(arquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Entity.CaminhoArquivo = "ArquivoCapaRedeSocial/";
                            Entity.NomeArquivo = NovoNomeArquivo;
                        }
                        int Width = 0;
                        int Height = 0;
                        using (var image = Image.FromStream(file.OpenReadStream()))
                        {
                            Width = image.Width;
                            Height = image.Height;
                        }

                        CapaModel.Update(new Capa()
                        {
                            IdCapa = Entity.IdCapa,
                            Titulo = Entity.Titulo,
                            CaminhoArquivo = Entity.CaminhoArquivo,
                            NomeArquivo = Entity.NomeArquivo,
                            Width = Width,
                            Height = Height,
                            DataCadastro = DateTime.Now,
                            Ativo = Entity.Ativo
                        });
                    }

                    EntityLista = CapaModel.GetList();
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

        public IActionResult OnPostExcluirImagem(IFormCollection dados)
        {
            List<CapaViewModel> EntityLista = null;
            bool status = false;
            string mensagem = "";
            
            try
            {
                Capa EntityCapa = null;
                EntityCapa = CapaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(dados["IdCapa"])).ExtractLong());
                CapaModel.Update(new Capa()
                {
                    IdCapa = EntityCapa.IdCapa,
                    Titulo = EntityCapa.Titulo,
                    CaminhoArquivo = EntityCapa.CaminhoArquivo,
                    NomeArquivo = EntityCapa.NomeArquivo,
                    Width = EntityCapa.Width,
                    Height = EntityCapa.Height,
                    DataCadastro = EntityCapa.DataCadastro,
                    Ativo = false
                });                
                EntityLista = CapaModel.GetList();
                status = true;
                mensagem = "Capa Excluída com Sucesso!";
            }
            catch (Exception ex)
            {
                return new JsonResult( new {status = status, entityLista = EntityLista,  mensagem = ex.Message });
            }
            return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = mensagem });
        }

        public IActionResult OnPostObter(string IdCapa)
        {
            Capa EntityCapa = null;
            CapaRedeSocial EntityCapaRedeSocial = null;
            RedeSocial EntityRedeSocial = null;
            bool status = false;
            try
            {
                EntityCapa = CapaModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCapa)).ExtractLong());
                EntityCapaRedeSocial = CapaRedeSocialModel.Get(EntityCapa.IdCapa);
                EntityRedeSocial = RedeSocialModel.Get(EntityCapaRedeSocial.IdRedeSocial.ToString().ExtractLong());
                status = true;
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, ex.Message);
                return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityCapa, entityRedeSocial = EntityRedeSocial });
            }
            return new JsonResult(new { status = status, entityCapa = EntityCapa, entityRedeSocial = EntityRedeSocial, idCampanha = EntityCapaRedeSocial.IdCampanha });
        }
        public void CarregarLists()
        {
            ListRedeSocial = RedeSocialModel.GetList();
            ListCapa = CapaModel.GetList();
            ListCalendario = Business.CalendarioModel.GetList();
            ListCampanha = Business.CampanhaModel.GetList();
        }
    #endregion
}
}
