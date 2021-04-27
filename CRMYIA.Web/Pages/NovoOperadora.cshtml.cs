using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CRMYIA.Web.Pages
{
    public class NovoOperadoraModel : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }
        //[BindProperty]
        //public IFormFile NomeArquivoBanner { get; set; }

        [BindProperty]
        public Operadora Entity { get; set; }
        [BindProperty]
        public List<GrupoCorretor> ListGrupoCorretor { get; set; }
        [BindProperty]
        public List<GrupoCorretorOperadora> ListGrupoCorretorOperadora { get; set; }

        #endregion

        #region Construtores
        public NovoOperadoraModel(IConfiguration configuration, IHostingEnvironment environment)
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
                Entity = new Operadora();
            }
            else
            {
                Entity = OperadoraModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                ListGrupoCorretorOperadora = GrupoCorretorOperadoraModel.Get(Entity.IdOperadora);
            }
            Entity.DataCadastro = DateTime.Now;

            CarregarLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string[] IdGrupoCorretores = Request.Form["IdGrupoCorretor"];
                bool gravado = false;

                //var File = Request.Form.Files.ToList();
                var File = Request.Form.Files.FirstOrDefault();
                string NomeArquivoOriginal = File.FileName;
                string NomeArquivo = Request.Form["Entity.NomeArquivo"];

                if (Entity.IdOperadora == 0)
                {
                    

                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoOperadora"], NomeArquivo);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await File.CopyToAsync(fileStream);
                    }

                    Entity.CaminhoArquivo = "ArquivoOperadora/";
                    Entity.NomeArquivo = NomeArquivo;

                    OperadoraModel.Add(Entity);

                    Entity = OperadoraModel.GetLastId();
                    foreach (var Item in IdGrupoCorretores)
                    {
                        if (Item != null)
                            Business.GrupoCorretorOperadoraModel.Add(new GrupoCorretorOperadora()
                            {
                                IdGrupoCorretor = (byte)Convert.ToInt32(Item),
                                IdOperadora = Entity.IdOperadora
                            });
                    }
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                }
                else
                {

                    if (File.Length > 0 && NomeArquivoOriginal != null)
                    {
                        

                        if (!string.IsNullOrEmpty(NomeArquivo) && File != null)
                        {
                            string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoOperadora"], NomeArquivo);
                            if ((System.IO.File.Exists(_imageToBeDeleted)))
                            {
                                System.IO.File.Delete(_imageToBeDeleted);
                            }
                            NomeArquivoOriginal = File.FileName;

                            string NovoNomeArquivo = Util.TratarNomeArquivoSeparadorPipe(NomeArquivoOriginal, 0);
                            var arquivo = Path.Combine(_environment.WebRootPath, _configuration["ArquivoOperadora"], NovoNomeArquivo);

                            using (var fileStream = new FileStream(arquivo, FileMode.Create))
                            {
                                await File.CopyToAsync(fileStream);
                                Entity.CaminhoArquivo = "ArquivoOperadora/";
                                Entity.NomeArquivo = NovoNomeArquivo;
                            }
                            OperadoraModel.Update(Entity);
                        }

                        List<GrupoCorretorOperadora> ListEntityGrupoCorretorOperadora = null;
                        ListEntityGrupoCorretorOperadora = GrupoCorretorOperadoraModel.Get(Entity.IdOperadora);

                        for (var i = 0; i < IdGrupoCorretores.Length; i++)
                        {
                            Business.GrupoCorretorOperadoraModel.Add(new GrupoCorretorOperadora()
                            {
                                IdGrupoCorretor = (byte)Convert.ToInt32(IdGrupoCorretores[i]),
                                IdOperadora = Entity.IdOperadora
                            });
                            if (i >= IdGrupoCorretores.Length - 1)
                            {
                                gravado = true;
                            }
                        }

                        if (gravado)
                        {
                            foreach (var Item in ListEntityGrupoCorretorOperadora)
                            {
                                Business.GrupoCorretorOperadoraModel.Delete(Item);
                            }
                        }
                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados alterados com sucesso!");
                        ListGrupoCorretorOperadora = GrupoCorretorOperadoraModel.Get(Entity.IdOperadora);
                        CarregarLists();
                    }
                    else
                    {
                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Informe uma Imagem");
                    }
                    
                }

                
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }

        public void CarregarLists()
        {
            ListGrupoCorretor = Business.GrupoCorretorModel.GetList();
        }
        #endregion
    }
}
