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
    public class UploadCampanhaGenerica : PageModel
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
        public List<MaterialDivulgacaoViewModel> ListEntity { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }
        [BindProperty]
        public string CaminhoImagem { get; set; }
        [BindProperty]
        public List<Calendario> ListCalendario { get; set; }
        #endregion

        #region Construtores
        public UploadCampanhaGenerica(IConfiguration configuration, IHostingEnvironment environment)
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
                Entity = new CampanhaArquivo();
                Entity.DataCadastro = DateTime.Now;
            }
            else
            {
                Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ImagemDiferente = Entity.NomeArquivo;
            }
            if (Entity.CaminhoArquivo != null && Entity.NomeArquivo != null)
            {
                CaminhoImagem = Entity.CaminhoArquivo + Entity.NomeArquivo;
            }
            else
            {
                CaminhoImagem = "/img/fotoCadastro/foto-cadastro.jpeg";
            }
            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadCampanhaGenericaAsync([FromForm] List<ICollection<IFormFile>> files, EnviarCampanhaArquivoViewModel formData)//long IdCampanhaArquivo, long IdCampanha, string Descricao, string Observacao, bool Ativo)
        {
            try
            {
                var documentFile = Request.Form.Files.ToList();
                List<MaterialDivulgacaoViewModel> EntityLista = null;
                CampanhaArquivo EntityCampanhaArquivo = null;
                Informacao EntityInformacao = null;
                MensagemModel mensagem = null;
                List<string> FilesNomes = new List<string>();
                bool status = false;
                
                    foreach (IFormFile Item in documentFile)
                    {
                        FilesNomes.Add(Item.FileName);
                    }

                    if (formData.Descricao == null && documentFile == null)
                    {
                        mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Por favor verifique o formulário.");
                    }
                    else if (!Util.VerificaNomeArquivo(FilesNomes))
                    {
                        mensagem = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Nome do arquivo não esta no padrão!");
                    }
                    else
                    {
                        if (mensagem == null)
                        {
                            var msg = "";
                            if (formData.IdCampanhaArquivo == 0)
                            {
                                msg = "Carregado";
                                int i = 0;
                                string NomeArquivo = string.Empty;
                                int Width = 0;
                                int Height = 0;
                                string RedesSociais = string.Empty;
                                string TipoPostagem = string.Empty;
                                string[] VetFileName;
                                string[] VetRedesSociais;
                                string[] VetTipoPostagem;

                                InformacaoModel.Add(new Informacao() { 
                                    Titulo = formData.Titulo,
                                    Descricao = formData.Descricao,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });

                                EntityInformacao = InformacaoModel.GetLastId();

                                foreach (var Item in documentFile)
                                {
                                    string NomeArquivoOriginal = Item.FileName;

                                    NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                    var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanhaArquivo"], NomeArquivo);

                                    using (var fileStream = new FileStream(file, FileMode.Create))
                                    {
                                        await Item.CopyToAsync(fileStream);
                                        CaminhoImagem = "ArquivoCampanhaArquivo/";
                                        RedesSociais = string.Empty;
                                        VetFileName = Item.FileName.Split('-');
                                        RedesSociais = VetFileName[1];

                                        TipoPostagem = string.Empty;
                                        TipoPostagem = VetFileName[2];
                                    }
                                    using (var image = Image.FromStream(Item.OpenReadStream()))
                                    {
                                        Width = image.Width;
                                        Height = image.Height;
                                    }
                                    i++;

                                    CampanhaArquivoModel.Add(new CampanhaArquivo()
                                    {
                                        IdCampanha = formData.IdCampanha,
                                        IdInformacao = EntityInformacao.IdInformacao,
                                        IdCalendario = formData.IdCalendario,
                                        CaminhoArquivo = "ArquivoCampanhaArquivo/",
                                        NomeArquivo = NomeArquivo,
                                        Width = Width,
                                        Height = Height,
                                        RedesSociais = RedesSociais,
                                        TipoPostagem = TipoPostagem,
                                        DataCadastro = DateTime.Now,
                                        Ativo = formData.Ativo
                                    });
                                }
                                EntityLista = CampanhaArquivoModel.GetList();
                                status = true;
                                mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                            }else
                            {
                            //UpDate Texto Informacao
                                EntityCampanhaArquivo = CampanhaArquivoModel.Get(formData.IdCampanhaArquivo);
                                EntityInformacao = InformacaoModel.Get(formData.IdInformacao);

                                if (EntityCampanhaArquivo != null && EntityInformacao != null) {

                                    InformacaoModel.Update(new Informacao() { 
                                        IdInformacao = formData.IdInformacao,
                                        Titulo = formData.Titulo,
                                        Descricao = formData.Descricao,
                                        DataCadastro = DateTime.Now,
                                        Ativo = formData.Ativo
                                    });

                                    CampanhaArquivoModel.Update(new CampanhaArquivo() {
                                        IdCampanhaArquivo = formData.IdCampanhaArquivo,
                                        IdCampanha = formData.IdCampanha,
                                        IdInformacao = formData.IdInformacao,
                                        IdCalendario = formData.IdCalendario,
                                        CaminhoArquivo = EntityCampanhaArquivo.CaminhoArquivo,
                                        NomeArquivo = EntityCampanhaArquivo.NomeArquivo,
                                        Width = EntityCampanhaArquivo.Width,
                                        Height = EntityCampanhaArquivo.Height,
                                        RedesSociais = EntityCampanhaArquivo.RedesSociais,
                                        TipoPostagem = EntityCampanhaArquivo.TipoPostagem,
                                        DataCadastro = DateTime.Now,
                                        Ativo = formData.Ativo
                                    }); 

                                }

                                mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Registro atualizado com Sucesso!!!");
                                EntityLista = CampanhaArquivoModel.GetList();
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
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
           
        }

        public async Task<IActionResult> OnPostAlterarImagemAsync()
        {
            try
            {
                List<MaterialDivulgacaoViewModel> EntityLista = null;
                bool status = false;
                MensagemModel mensagem = null;

                var file = Request.Form.Files.FirstOrDefault();
                string IdCampanhaArquivo = Request.Form["IdCampanhaArquivo"].ToString();
                string NomeArquivo = Request.Form["NomeArquivo"].ToString();
                CampanhaArquivo Entity = null;
                Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCampanhaArquivo)).ExtractLong());
                string msg = "Alterado";
                int Width = 0;
                int Height = 0;
                string RedesSociais = string.Empty;
                string[] VetFileName;
                string TipoPostagem = string.Empty;
                string NovoNomeArquivo = string.Empty;

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (Util.VerificaNomeArquivo(FilesNomes))
                {
                    if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                    {
                        string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanhaArquivo"], NomeArquivo);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        string NomeArquivoOriginal = file.FileName;

                        NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                        var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanhaArquivo"], NovoNomeArquivo);
                       

                        string[] vetNomeArquivo = Entity.NomeArquivo.Split(";");

                        Entity.NomeArquivo = "";
                       
                        using (var fileStream = new FileStream(arquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Entity.CaminhoArquivo = "ArquivoCampanhaArquivo/";
                            Entity.NomeArquivo = 
                            RedesSociais = string.Empty;
                            VetFileName = file.FileName.Split('-');
                            RedesSociais = VetFileName[1];
                            TipoPostagem = string.Empty;
                            TipoPostagem = VetFileName[2];
                        }
                        using (var image = Image.FromStream(file.OpenReadStream()))
                        {
                            Width = image.Width;
                            Height = image.Height;
                        }

                    }
                    else
                    {
                        Entity.CaminhoArquivo = "img/fotoCadastro/";
                        Entity.NomeArquivo = "foto-cadastro.jpeg";
                    }
                    CampanhaArquivoModel.Update(new CampanhaArquivo()
                    {
                        IdCampanhaArquivo = Entity.IdCampanhaArquivo,
                        IdCampanha = Entity.IdCampanha,
                        IdInformacao = Entity.IdInformacao,
                        CaminhoArquivo = Entity.CaminhoArquivo,
                        NomeArquivo = NovoNomeArquivo,
                        Width = Width,
                        Height = Height,
                        RedesSociais = RedesSociais,
                        TipoPostagem = TipoPostagem,
                        DataCadastro = DateTime.Now,
                        Ativo = Entity.Ativo
                    });
                    
                    EntityLista = CampanhaArquivoModel.GetList();
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

        public IActionResult OnPostExcluirImagem(string IdCampanhaArquivo)
        {
            try
            {
                List<MaterialDivulgacaoViewModel> EntityLista = null;
                CampanhaArquivo Entity = null;
                bool status = false;
                string Mensagem = "";
                Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCampanhaArquivo)).ExtractLong());

                Entity.Ativo = false;
                CampanhaArquivoModel.Update(Entity);
                EntityLista = CampanhaArquivoModel.GetList();
                status = true;
                Mensagem = "Imagem Excluída com Sucesso!";
                
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = Mensagem });
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
        }
        public IActionResult OnPostObter(string IdCampanhaarquivo)
        {

            CampanhaArquivo EntityCampanhaArquivo = null;
            Informacao EntityInformacao = null;
            bool status = false;
            Mensagem = null;
            try
            {
                EntityCampanhaArquivo = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCampanhaarquivo)).ExtractLong());
                EntityInformacao = InformacaoModel.Get(EntityCampanhaArquivo.IdInformacao.ToString().ExtractLong());

                status = true;
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, ex.Message);
                return new JsonResult(new { status = status, mensagem = Mensagem, entityLista = EntityCampanhaArquivo, entityInformacao = EntityInformacao });
            }
            return new JsonResult(new { status = status, entityLista = EntityCampanhaArquivo, entityInformacao = EntityInformacao });
        }

        public void CarregarLists()
        {
            ListEntity = CampanhaArquivoModel.GetList();
            ListCalendario = Business.CalendarioModel.GetList();
            ListCampanha = Business.CampanhaModel.GetList();
        }
        #endregion
    }
}
