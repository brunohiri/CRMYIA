using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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
    public class NovoUsuarioModel : PageModel
    {
        #region Propriedades
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public MensagemModel Mensagem { get; set; }

        [BindProperty]
        public IFormFile NomeArquivoFoto { get; set; }
        [BindProperty]
        public string ImagemDiferente { get; set; }

        [BindProperty]
        public Usuario Entity { get; set; }

        [BindProperty]
        public string ConfirmarSenha { get; set; }

        [BindProperty]
        public byte? UsuarioIdPerfil { get; set; }

        [BindProperty]
        public long? IdUsuarioHierarquia { get; set; }


        [BindProperty]
        public List<Perfil> ListPerfil { get; set; }

        #endregion

        #region Construtores
        public NovoUsuarioModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #endregion

        #region Métodos
        public IActionResult OnGet(string Id = null)
        {
            if (Id.IsNullOrEmpty())
                Entity = new Usuario();
            else
            {
                Entity = UsuarioModel.Get(Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong());
                if (!Entity.Senha.IsNullOrEmpty())
                {
                    Entity.Senha = Criptography.Decrypt(Entity.Senha);
                    ConfirmarSenha = Entity.Senha;
                }
                UsuarioIdPerfil = Entity.UsuarioPerfil != null && Entity.UsuarioPerfil.Count() > 0 ? Entity.UsuarioPerfil.FirstOrDefault().IdPerfil : null;
                IdUsuarioHierarquia =  Entity.UsuarioHierarquiaIdUsuarioSlaveNavigation != null && Entity.UsuarioHierarquiaIdUsuarioSlaveNavigation.Count() > 0 ? Entity.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMaster : null;
            }

            Entity.DataCadastro = DateTime.Now;
            ListPerfil = PerfilModel.GetListIdDescricao();
            return Page();
        }

        public IActionResult OnGetPerfil(byte idPerfil)
        {
            List<Usuario> ListUsuario = null;
            try
            {
                if (idPerfil == (byte)EnumeradorModel.Perfil.Corretor)
                    idPerfil = (byte)EnumeradorModel.Perfil.Supervisor;
                else
                if (idPerfil == (byte)EnumeradorModel.Perfil.Supervisor)
                    idPerfil = (byte)EnumeradorModel.Perfil.Gerente;

                ListUsuario = UsuarioModel.GetList(idPerfil);
            }
            catch (Exception)
            {

                throw;
            }
            return new JsonResult(new { status = true, List = ListUsuario.Select(x => new { IdUsuario = x.IdUsuario, Nome = x.Nome }).ToList() });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ListPerfil = PerfilModel.GetListIdDescricao();

                if ((!Util.IsCpf(Entity.Documento)) && (!Util.IsCnpj(Entity.Documento)))
                    Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "CPF ou CNPJ Inválido!");
                else
                {
                    if (!Entity.Senha.IsNullOrEmpty())
                    {
                        EnumeradorModel.PasswordStrength classicacaoSenha = Util.GetPasswordStrength(Entity.Senha);
                        if ((classicacaoSenha != EnumeradorModel.PasswordStrength.Aceitavel)
                            && (classicacaoSenha != EnumeradorModel.PasswordStrength.Forte)
                            && (classicacaoSenha != EnumeradorModel.PasswordStrength.Segura))
                        {
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, string.Format("Sua senha foi considerada {0}! Melhore sua senha! Utilize números, caracteres especiais e letras maiúsculas e minúsculas!", classicacaoSenha.ToString()));
                        }
                        else
                        if (Entity.Senha != ConfirmarSenha)
                        {
                            Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Senha não é compatível com a confirmação de senha!");
                        }
                    }

                    if (Mensagem == null)
                    {
                        Entity.IP = HttpContext.Connection.RemoteIpAddress.ToString();

                        if (Entity.IdUsuario == 0)
                        {
                            if (UsuarioModel.GetByDocumento(Entity.Documento) != null)
                                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um usuário cadastrado com este CPF/CNPJ!");
                            else
                            if (UsuarioModel.GetByLogin(Entity.Login) != null)
                                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Aviso, "Já existe um usuário cadastrado com este Login!");
                            else
                            {
                                string NomeArquivoOriginal = NomeArquivoFoto.FileName;
                                string NomeArquivo = string.Empty;
                                NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoFoto"], NomeArquivo);
                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await NomeArquivoFoto.CopyToAsync(fileStream);
                                }
                                Entity.CaminhoFoto = Path.Combine(_environment.WebRootPath, _configuration["ArquivoFoto"]);
                                Entity.NomeFoto = NomeArquivo;
                                UsuarioModel.Add(Entity);
                                UsuarioPerfilModel.Add(new UsuarioPerfil() { IdUsuario = Entity.IdUsuario, IdPerfil = UsuarioIdPerfil, Ativo = true });
                                if (IdUsuarioHierarquia.HasValue)
                                {
                                    UsuarioHierarquiaModel.Add(new UsuarioHierarquia() { IdUsuarioSlave = Entity.IdUsuario, IdUsuarioMaster = IdUsuarioHierarquia, DataCadastro = DateTime.Now, Ativo = true });
                                }
                            }
                        }
                        else
                        {
                            string NomeArquivo = string.Empty;
                            if (!string.IsNullOrEmpty(ImagemDiferente) && NomeArquivoFoto != null)
                            {
                                string _imageToBeDeleted = Path.Combine(_environment.WebRootPath, _configuration["ArquivoFoto"], ImagemDiferente);
                                if ((System.IO.File.Exists(_imageToBeDeleted)))
                                {
                                    System.IO.File.Delete(_imageToBeDeleted);
                                }
                                string NomeArquivoOriginal = NomeArquivoFoto.FileName;

                                NomeArquivo = Util.TratarNomeArquivo(NomeArquivoOriginal, 0);
                                var file = Path.Combine(_environment.WebRootPath, _configuration["ArquivoFoto"], NomeArquivo);
                                using (var fileStream = new FileStream(file, FileMode.Create))
                                {
                                    await NomeArquivoFoto.CopyToAsync(fileStream);
                                }
                            }
                            else
                            {
                                NomeArquivo = ImagemDiferente;
                            }
                            if (!Entity.Senha.IsNullOrEmpty())
                                Entity.Senha = Criptography.Encrypt(Entity.Senha);
                            UsuarioModel.Update(Entity);
                            #region Verifica UsuarioPerfil
                            if (UsuarioIdPerfil.HasValue)
                            {
                                UsuarioPerfil usuarioPerfil = UsuarioPerfilModel.Get(Entity.IdUsuario);
                                if (usuarioPerfil == null)
                                    UsuarioPerfilModel.Add(new UsuarioPerfil() { IdUsuario = Entity.IdUsuario, IdPerfil = UsuarioIdPerfil, Ativo = true });
                                else
                                {
                                    usuarioPerfil.IdPerfil = UsuarioIdPerfil;
                                    UsuarioPerfilModel.Update(usuarioPerfil);
                                }
                            }
                            #endregion

                            #region Verifica UsuarioHierarquia
                            if (IdUsuarioHierarquia.HasValue)
                            {
                                UsuarioHierarquia usuarioHierarquia = UsuarioHierarquiaModel.Get(Entity.IdUsuario);
                                if (usuarioHierarquia == null)
                                    UsuarioHierarquiaModel.Add(new UsuarioHierarquia() { IdUsuarioSlave = Entity.IdUsuario, IdUsuarioMaster = IdUsuarioHierarquia, DataCadastro = DateTime.Now, Ativo = true });
                                else
                                {
                                    usuarioHierarquia.IdUsuarioSlave = Entity.IdUsuario;
                                    usuarioHierarquia.IdUsuarioMaster = IdUsuarioHierarquia;
                                    usuarioHierarquia.DataCadastro = DateTime.Now;
                                    UsuarioHierarquiaModel.Update(usuarioHierarquia);
                                }
                            }
                            #endregion
                        }


                        Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Sucesso, "Dados salvos com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                Mensagem = new MensagemModel(Business.Util.EnumeradorModel.TipoMensagem.Erro, "Erro ao salvar! Erro: " + ex.Message.ToString());
            }
            return Page();
        }
        #endregion
    }
}
