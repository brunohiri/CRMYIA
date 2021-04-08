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
                if (Entity.IdOperadora == 0)
                {
                    var File = Request.Form.Files.ToList();
                    string NomeArquivoOriginal = File[0].FileName;
                    string NomeArquivo = string.Empty;

                    NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                    var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoOperadora"], NomeArquivo);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await File[0].CopyToAsync(fileStream);
                    }

                    Entity.CaminhoArquivo = "ArquivoOperadora/";
                    Entity.NomeArquivo = NomeArquivo;

                    OperadoraModel.Add(Entity);

                    Entity = OperadoraModel.GetLastId();
                    foreach (var Item in IdGrupoCorretores)
                    {
                        if (Item != null)
                            Business.GrupoCorretorCampanhaModel.Add(new GrupoCorretorCampanha()
                            {
                                IdGrupoCorretor = (byte)Convert.ToInt32(Item),
                                IdCampanha = Entity.IdOperadora
                            });
                    }
                }
                else
                {
                    OperadoraModel.Update(Entity);

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
                }

                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
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
