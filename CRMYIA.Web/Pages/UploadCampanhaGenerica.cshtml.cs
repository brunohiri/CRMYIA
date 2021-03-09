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

        public async Task<IActionResult> OnPostUploadCampanhaGenericaAsync([FromForm] List<ICollection<IFormFile>> files, CampanhaArquivo formData)//long IdCampanhaArquivo, long IdCampanha, string Descricao, string Observacao, bool Ativo)
        {
            try
            {

                
                var documentFile = Request.Form.Files.ToList();
                List<MaterialDivulgacaoViewModel> EntityLista = null;
                MensagemModel mensagem = null;
                List<string> FilesNomes = new List<string>();
                bool status = false;
                //if (CampanhaArquivoModel.GetCampanhaId(formData.IdCampanha.ToString().ExtractLong()))
                
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
                                foreach (var Item in documentFile)
                                {
                                    string NomeArquivoOriginal = Item.FileName;

                                    NomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                                    var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanhaArquivo"], NomeArquivo);

                                    using (var fileStream = new FileStream(file, FileMode.Create))
                                    {
                                        await Item.CopyToAsync(fileStream);
                                        formData.CaminhoArquivo = "ArquivoCampanhaArquivo/";
                                        if (i + 1 < documentFile.Count)
                                        {
                                            formData.NomeArquivo += NomeArquivo + "|";
                                        }
                                        else
                                        {
                                            formData.NomeArquivo += NomeArquivo;
                                        }
                                        CaminhoImagem = "ArquivoCampanhaArquivo/";
                                        RedesSociais = string.Empty;
                                        VetFileName = Item.FileName.Split('-');
                                        RedesSociais = VetFileName[1];
                                        //foreach (var ItemRedesSociais in VetRedesSociais)
                                        //{
                                        //    RedesSociais += ItemRedesSociais + "|";
                                        //}

                                        TipoPostagem = string.Empty;
                                        TipoPostagem = VetFileName[2];
                                        //foreach (var ItemTipoPostagem in VetTipoPostagem)
                                        //{
                                        //    TipoPostagem += ItemTipoPostagem + "|";
                                        //}
                                    }
                                    using (var image = Image.FromStream(Item.OpenReadStream()))
                                    {
                                        Width = image.Width;
                                        Height = image.Height;
                                    }

                                    if (i + 1 < documentFile.Count)
                                    {
                                        formData.Width += Width + "|";
                                        formData.Height += Height + "|";
                                    }
                                    else
                                    {
                                        formData.Width += Width;
                                        formData.Height += Height;
                                    }
                                    i++;


                                }
                                CampanhaArquivoModel.Add(new CampanhaArquivo()
                                {
                                    IdCampanha = formData.IdCampanha,
                                    Descricao = formData.Descricao,
                                    CaminhoArquivo = "ArquivoCampanhaArquivo/",
                                    NomeArquivo = formData.NomeArquivo,
                                    Width = formData.Width,
                                    Height = formData.Height,
                                    RedesSociais = RedesSociais,
                                    TipoPostagem = TipoPostagem,
                                    DataCadastro = DateTime.Now,
                                    Ativo = formData.Ativo
                                });

                                EntityLista = CampanhaArquivoModel.GetList();
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

                List<string> FilesNomes = new List<string>();

                FilesNomes.Add(file.FileName);

                if (Util.VerificaNomeArquivo(FilesNomes))
                {
                    if (!string.IsNullOrEmpty(NomeArquivo) && file != null)
                    {
                        string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NomeArquivo);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        string NomeArquivoOriginal = file.FileName;

                        string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                        var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoCampanha"], NovoNomeArquivo);

                        string[] vetNomeArquivo = Entity.NomeArquivo.Split(";");

                        Entity.NomeArquivo = "";
                        for (int i = 0; i < vetNomeArquivo.Length; i++)
                        {
                            if (vetNomeArquivo[i] == NomeArquivo)
                            {
                                vetNomeArquivo[i] = NovoNomeArquivo;
                            }

                            if (i + 1 < vetNomeArquivo.Length)
                            {
                                Entity.NomeArquivo += vetNomeArquivo[i] + "|";
                            }
                            else
                            {
                                Entity.NomeArquivo += vetNomeArquivo[i];
                            }
                        }

                        using (var fileStream = new FileStream(arquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Entity.CaminhoArquivo = "ArquivoCampanhaArquivo/";
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
                        Descricao = Entity.Descricao,
                        CaminhoArquivo = Entity.CaminhoArquivo,
                        NomeArquivo = Entity.NomeArquivo,
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

        public IActionResult OnGetExcluirImagem(string IdCampanhaArquivo, string NomeArquivo)
        {
            try
            {
                List<MaterialDivulgacaoViewModel> EntityLista = null;
                CampanhaArquivo Entity = null;
                bool status = false;
                string Mensagem = "";
                Entity = CampanhaArquivoModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(IdCampanhaArquivo)).ExtractLong());
                var AuxNomeImagem = "";
                var NomeImagem = Entity.NomeArquivo.Split(";");
                int Tam = NomeImagem.Length;
                int i = 0;
                if (Tam > 1)
                {
                    foreach (var Item in NomeImagem)
                    {
                        if (Item == NomeArquivo)
                        {
                            if (i + 1 < Tam)
                            {
                                AuxNomeImagem += "#" + Item + ";";
                            }
                            else
                            {
                                AuxNomeImagem += "#" + Item;
                            }
                        }
                        else
                        {
                            if (i + 1 < Tam)
                            {
                                AuxNomeImagem += Item + ";";
                            }
                            else
                            {
                                AuxNomeImagem += Item;
                            }
                            
                        }
                        i++;
                    }

                    Entity.NomeArquivo = AuxNomeImagem;
                    CampanhaArquivoModel.Update(Entity);
                    EntityLista = CampanhaArquivoModel.GetList();
                    status = true;
                    Mensagem = "Imagem Excluída com Sucesso!";
                }
                else
                {
                    Entity.Ativo = false;
                    CampanhaArquivoModel.Update(Entity);
                    EntityLista = CampanhaArquivoModel.GetList();
                    status = true;
                    Mensagem = "Imagem Excluída com Sucesso!";
                }
                return new JsonResult(new { status = status, entityLista = EntityLista, mensagem = Mensagem });
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
        }
        public void CarregarLists()
        {
            ListEntity = CampanhaArquivoModel.GetList();
            ListCampanha = Business.CampanhaModel.GetList();
        }
        #endregion
    }
}
