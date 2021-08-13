using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using CRMYIA.Business.Util;
using System.Web;

namespace CRMYIA.Business
{
    public class UsuarioModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Usuario Get(long IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                        .Include(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                        .Where(x => x.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static Usuario GetByDocumento(string Documento = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.Documento == Documento)
                        //?.AsNoTracking()
                        ?.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Usuario GetByEmail(string Email = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.Email.ToLower() == Email.ToLower())
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Usuario GetByLogin(string Login = null)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Where(x => x.Login.ToLower() == Login.ToLower())
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Usuario GetUsuariosMaster(long IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Include(y => y.UsuarioHierarquiaIdUsuarioMasterNavigation)
                        .Where(x => x.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Usuario GetUsuarioSlave(long? IdUsuario)
        {
            Usuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Usuario
                        .Include(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                        .Where(x => x.IdUsuario == IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static List<Usuario> GetAllUsuarioSlave(ICollection<UsuarioHierarquia> usuariosHierarquia)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (var usuarioHierarquia in usuariosHierarquia)
            {
                usuarios.Add(Get((long)usuarioHierarquia.IdUsuarioSlave));
            }
            return usuarios;
        }

        public static List<UsuarioViewModel> GetList(bool? Ativo, string? Descricao, DateTime? DataInicio, DateTime? DataFim, bool DataValida)
        {
            List<UsuarioViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    if (Ativo == null && Descricao == null)
                    {
                        ListEntity = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                        .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .AsNoTracking()
                        .Where(x => (x.DataCadastro >= DataInicio && x.DataCadastro <= DataFim))
                        .Select(x => new UsuarioViewModel()
                        {
                            IdUsuario = x.IdUsuario,
                            IdUsuarioString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdUsuario.ToString())),
                            Nome = x.Nome,
                            Email = x.Email,
                            Telefone = x.Telefone,
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo,
                            UsuarioPerfil = x.UsuarioPerfil
                        })
                        .ToList();
                    }
                    else if (Ativo != null && Descricao != null)
                    {
                        ListEntity = context.Usuario
                       .Include(y => y.UsuarioPerfil)
                       .ThenInclude(p => p.IdPerfilNavigation)
                       .Include(c => c.IdCorretoraNavigation)
                       .AsNoTracking()
                       .Where(x => x.Ativo == Ativo && (x.UsuarioPerfil != null && x.UsuarioPerfil.Count > 0 && x.UsuarioPerfil.First().IdPerfilNavigation != null && x.UsuarioPerfil.First().IdPerfilNavigation.Descricao.Contains(Descricao) == true))
                       .Select(x => new UsuarioViewModel()
                       {
                           IdUsuario = x.IdUsuario,
                           IdUsuarioString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdUsuario.ToString())),
                           Nome = x.Nome,
                           Email = x.Email,
                           Telefone = x.Telefone,
                           DataCadastro = x.DataCadastro,
                           Ativo = x.Ativo,
                           UsuarioPerfil = x.UsuarioPerfil
                       })
                       .ToList();
                    }
                    else if (Ativo != null && Descricao == null)
                    {
                        ListEntity = context.Usuario
                       .Include(y => y.UsuarioPerfil)
                       .ThenInclude(p => p.IdPerfilNavigation)
                       .Include(c => c.IdCorretoraNavigation)
                       .AsNoTracking()
                       .Where(x => x.Ativo == Ativo)
                       .Select(x => new UsuarioViewModel()
                       {
                           IdUsuario = x.IdUsuario,
                           IdUsuarioString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdUsuario.ToString())),
                           Nome = x.Nome,
                           Email = x.Email,
                           Telefone = x.Telefone,
                           DataCadastro = x.DataCadastro,
                           Ativo = x.Ativo,
                           UsuarioPerfil = x.UsuarioPerfil
                       })
                       .ToList();
                    }
                    else if (Ativo == null && Descricao != null)
                    {
                        ListEntity = context.Usuario
                       .Include(y => y.UsuarioPerfil)
                       .ThenInclude(p => p.IdPerfilNavigation)
                       .Include(c => c.IdCorretoraNavigation)
                       .AsNoTracking()
                       .Where(x => (x.UsuarioPerfil != null && x.UsuarioPerfil.Count > 0 && x.UsuarioPerfil.First().IdPerfilNavigation != null && x.UsuarioPerfil.First().IdPerfilNavigation.Descricao.Contains(Descricao) == true))
                       .Select(x => new UsuarioViewModel()
                       {
                           IdUsuario = x.IdUsuario,
                           IdUsuarioString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdUsuario.ToString())),
                           Nome = x.Nome,
                           Email = x.Email,
                           Telefone = x.Telefone,
                           DataCadastro = x.DataCadastro,
                           Ativo = x.Ativo,
                           UsuarioPerfil = x.UsuarioPerfil
                       })
                       .ToList();
                    }

                    if (DataValida)
                    {
                        ListEntity = ListEntity
                            .Where(x => (x.DataCadastro >= DataInicio && x.DataCadastro <= DataFim))
                            .ToList();

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Usuario> GetList()
        {
            List<Usuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                    .Include(y => y.UsuarioPerfil)
                    .ThenInclude(p => p.IdPerfilNavigation)
                    .Include(c => c.IdCorretoraNavigation)
                    .AsNoTracking()
                    .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<ListaCorretorViewModel> GetList(byte IdPerfil)
        {
            List<ListaCorretorViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                        .AsNoTracking()
                        .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil))
                        .Select(x => new ListaCorretorViewModel()
                        {
                            IdUsuario = x.IdUsuario,
                            Nome = x.Nome,
                            Email = x.Email,
                            Telefone = x.Telefone,
                            CaminhoFoto = x.CaminhoFoto,
                            NomeFoto = x.NomeFoto,
                            Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo
                        })
                        .OrderBy(o => o.Nome)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<ListaCorretorViewModel> GetList(byte IdPerfil, string Corretor = "", long Corretora = 0, long Supervisor = 0,
            long Gerente = 0, string DataInicial = "", string DataFinal = "", bool Status = true)
        {
            List<ListaCorretorViewModel> ListEntity = null;
            Corretora corretora = null;
            List<ListaCorretorViewModel> listContains = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    corretora = context.Corretora.Where(x => x.IdCorretora == Corretora).FirstOrDefault();

                    if (Corretor != "")
                    {
                        ListEntity = context.Usuario
                                    .Include(y => y.UsuarioPerfil)
                                        .ThenInclude(p => p.IdPerfilNavigation)
                                    .Include(c => c.IdCorretoraNavigation)
                                    .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                                    .AsNoTracking()
                                    .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil) && x.Nome.Contains(Corretor))
                                    .Select(x => new ListaCorretorViewModel()
                                    {
                                        IdUsuario = x.IdUsuario,
                                        Nome = x.Nome,
                                        Email = x.Email,
                                        Telefone = x.Telefone,
                                        CaminhoFoto = x.CaminhoFoto,
                                        NomeFoto = x.NomeFoto,
                                        Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                                        DataCadastro = x.DataCadastro,
                                        Ativo = x.Ativo
                                    })
                                    .OrderBy(o => o.Nome)
                                    .ToList();
                    }
                    else
                    {
                        ListEntity = context.Usuario
                                    .Include(y => y.UsuarioPerfil)
                                        .ThenInclude(p => p.IdPerfilNavigation)
                                    .Include(c => c.IdCorretoraNavigation)
                                    .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                                    .AsNoTracking()
                                    .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil))
                                    .Select(x => new ListaCorretorViewModel()
                                    {
                                        IdUsuario = x.IdUsuario,
                                        Nome = x.Nome,
                                        Email = x.Email,
                                        Telefone = x.Telefone,
                                        CaminhoFoto = x.CaminhoFoto,
                                        NomeFoto = x.NomeFoto,
                                        Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                                        DataCadastro = x.DataCadastro,
                                        Ativo = x.Ativo
                                    })
                                    .OrderBy(o => o.Nome)
                                    .ToList();
                    }
                    if (Corretora != 0 && corretora != null)
                    {
                        ListEntity.RemoveAll(x => x.Corretora != corretora.RazaoSocial);
                    }
                    if (DataInicial != "" && DataFinal != "")
                    {
                        ListEntity.RemoveAll(x => x.DataCadastro <= DateTime.Parse(DataInicial) && x.DataCadastro >= DateTime.Parse(DataFinal));
                    }
                    else if (DataInicial != "" && DataFinal == "")
                    {
                        ListEntity.RemoveAll(x => x.DataCadastro <= DateTime.Parse(DataInicial));
                    }
                    else if (DataInicial == "" && DataFinal != "")
                    {
                        ListEntity.RemoveAll(x => x.DataCadastro >= DateTime.Parse(DataFinal));
                    }
                    if (!Status)
                    {
                        ListEntity.RemoveAll(x => x.Ativo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static UsuarioCorretorViewModel GetUsuarioCorretor(long IdUsuario)
        {
            List<Usuario> ListEntity = GetList();
            UsuarioCorretorViewModel Entity = null;
            try
            {
                Entity = ListEntity
                    .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
                    .Select(x => new UsuarioCorretorViewModel()
                    {
                        Nome = x.Nome,
                        NomeApelido = x.NomeApelido,
                        Email = x.Email,
                        Telefone = x.Telefone
                    })
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static UsuarioSupervisorViewModel GetUsuarioSupervisor(long IdUsuario)
        {
            try
            {
                List<UsuarioHierarquia> hierarquiaList = UsuarioHierarquiaModel.GetAllUsuarioSlave(IdUsuario);

                using (YiaContext context = new YiaContext())
                {
                    return context.Usuario
                        .Where(u => u.IdUsuario == IdUsuario)
                        .Select(s => new UsuarioSupervisorViewModel()
                        {
                            IdUsuario = s.IdUsuario,
                            Nome = s.Nome,
                            NomeApelido = s.NomeApelido,
                            UsuariosCorretores = GetAllUsuarioSlave(hierarquiaList)
                                .Select(slave => new UsuarioCorretorViewModel()
                                {
                                    Nome = slave.Nome,
                                    NomeApelido = slave.NomeApelido,
                                    Email = slave.Email,
                                    Telefone = slave.Telefone
                                })
                                .ToList()
                        })
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static UsuarioGerenteViewModel GetUsuarioGerente(long IdUsuario)
        {
            try
            {
                List<UsuarioHierarquia> hierarquiaList = UsuarioHierarquiaModel.GetAllUsuarioSlave(IdUsuario);

                using (YiaContext context = new YiaContext())
                {
                    return context.Usuario
                        .Where(u => u.IdUsuario == IdUsuario)
                        .Select(s => new UsuarioGerenteViewModel()
                        {
                            IdUsuario = s.IdUsuario,
                            Nome = s.Nome,
                            NomeApelido = s.NomeApelido,
                            UsuariosSupervisores = GetAllUsuarioSlave(hierarquiaList)
                                .Select(slave => new UsuarioSupervisorViewModel()
                                {
                                    IdUsuario = slave.IdUsuario,
                                    Nome = slave.Nome,
                                    NomeApelido = slave.NomeApelido
                                })
                                .ToList()
                        })
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static KPIUsuarioViewModel GetKPIUsuario(long IdUsuario)
        {
            KPIUsuarioViewModel ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario.Where(x => x.IdUsuario == IdUsuario)
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                        .AsNoTracking()
                        .Select(x => new KPIUsuarioViewModel()
                        {
                            IdUsuario = x.IdUsuario,
                            Nome = x.Nome,
                            CaminhoFoto = x.CaminhoFoto,
                            NomeFoto = x.NomeFoto,
                            Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                            DescricaoPerfil = x.UsuarioPerfil.First().IdPerfilNavigation.Descricao,
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo
                        })
                        .OrderBy(o => o.Nome)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<ListaKPIUsuarioViewModel> GetListKPIUsuarioByName(byte IdPerfil, string perfil, string nome)
        {
            List<ListaKPIUsuarioViewModel> ListEntity = null;
            List<KPIGrupoUsuario> ListKPIGrupoUsuario = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {

                    ListKPIGrupoUsuario = context.KPIGrupoUsuario.Where(x => x.Grupo == true && x.Perfil == perfil).AsNoTracking().ToList();

                    ListEntity = context.Usuario.Include(i => i.IdClassificacaoNavigation)
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                        .AsNoTracking()
                        .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil) && x.Nome.Contains(nome))
                        .Select(x => new ListaKPIUsuarioViewModel()
                        {
                            IdUsuario = x.IdUsuario,
                            Nome = x.Nome,
                            CaminhoFoto = x.CaminhoFoto,
                            NomeFoto = x.NomeFoto,
                            Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                            DescricaoPerfil = x.UsuarioPerfil.First().IdPerfilNavigation.Descricao,
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo,
                            IdClassificacaoNavigation = x.IdClassificacaoNavigation
                        })
                        .OrderBy(o => o.Nome)
                        .ToList();

                    foreach (var item in ListKPIGrupoUsuario)
                    {
                        ListEntity.RemoveAll(us => us.Nome == item.Nome);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<ListaKPIUsuarioViewModel> GetListKPIUsuario(byte IdPerfil, string perfil)
        {
            List<ListaKPIUsuarioViewModel> ListEntity = null;
            List<KPIGrupoUsuario> ListKPIGrupoUsuario = null;
            List<KPIGrupo> ListKPIGrupo = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario.Include(i => i.IdClassificacaoNavigation)
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .Include(c => c.IdCorretoraNavigation)
                        .Include(h => h.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(uh => uh.IdUsuarioMasterNavigation)
                        .AsNoTracking()
                        .Where(x => x.UsuarioPerfil.Any(z => z.IdPerfil == IdPerfil))
                        .Select(x => new ListaKPIUsuarioViewModel()
                        {
                            IdUsuario = x.IdUsuario,
                            Nome = x.Nome,
                            CaminhoFoto = x.CaminhoFoto,
                            NomeFoto = x.NomeFoto,
                            Corretora = x.IdCorretoraNavigation == null ? "Sem Corretora" : x.IdCorretoraNavigation.RazaoSocial,
                            DescricaoPerfil = x.UsuarioPerfil.First().IdPerfilNavigation.Descricao,
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo,
                            IdClassificacaoNavigation = x.IdClassificacaoNavigation
                        })
                        .OrderBy(o => o.Nome)
                        .ToList();

                    if (perfil == "Supervisor")
                    {
                        ListKPIGrupo = context.KPIGrupo.Where(x => x.Ativo == true).AsNoTracking().ToList();
                        foreach (var item in ListKPIGrupo)
                        {
                            ListEntity.RemoveAll(us => us.Nome == item.Nome);
                        }
                    }
                    else
                    {
                        ListKPIGrupoUsuario = context.KPIGrupoUsuario.Where(x => x.Grupo == true && x.Ativo == true && x.Perfil == perfil).AsNoTracking().ToList();
                        foreach (var item in ListKPIGrupoUsuario)
                        {
                            ListEntity.RemoveAll(us => us.Nome == item.Nome);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<ListaAniversarianteViewModel> GetListAniversariante()
        {
            List<ListaAniversarianteViewModel> ListEntity = null;

            try
            {

                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                             .Include(y => y.UsuarioPerfil)
                                 .ThenInclude(u => u.IdPerfilNavigation)
                                     .ThenInclude(v => v.UsuarioPerfil)
                             .Where(x => x.DataNascimentoAbertura.HasValue ? (x.DataNascimentoAbertura.Value.Month == DateTime.Now.Month) : false)
                             .Select(x => new ListaAniversarianteViewModel()
                             {
                                 Nome = x.Nome.ToUpper(),
                                 UsuarioPerfil = x.UsuarioPerfil.First().IdPerfilNavigation.Descricao.ToUpper(),
                                 Telefone = x.Telefone,
                                 DataNascimentoAbertura = x.DataNascimentoAbertura
                             })
                             .OrderBy(o => o.DataNascimentoAbertura)
                             .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Usuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Usuario.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Usuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Usuario.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Usuario> GetListIdNome(long IdUsuario)
        {
            List<Usuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {

                    UsuarioPerfil up = context.UsuarioPerfil.Find(IdUsuario);


                    if (up.IdPerfil == 1)
                    {
                        ListEntity = context.Usuario
                        .Join(context.UsuarioPerfil, u => (Int64?)(u.IdUsuario), up => up.IdUsuario, (u, up) => u.Nome)
                        .Select(x => new Usuario()
                        {
                            Nome = x
                        })
                        .ToList();
                    }

                    if (up.IdPerfil == 2)
                    {
                        ListEntity = context.Usuario
                        .Join(context.UsuarioPerfil, u => (Int64?)(u.IdUsuario), up => up.IdUsuario, (u, up) => new { u = u, up = up })
                        .Where(temp0 => ((Int32?)(temp0.up.IdPerfil) != (Int32?)1))
                        .Select(temp0 => temp0.u.Nome)
                        .Select(x => new Usuario()
                        {
                            Nome = x
                        })
                        .ToList();
                    }

                    if (up.IdPerfil == 3)
                    {
                        ListEntity = context.Usuario
                        .Join(context.UsuarioPerfil, u => (Int64?)(u.IdUsuario), up => up.IdUsuario, (u, up) => new { u = u, up = up })
                        .Where(temp0 => ((Int32?)(temp0.up.IdPerfil) != (Int32?)1) && ((Int32?)(temp0.up.IdPerfil) != (Int32?)2))
                        .Select(temp0 => temp0.u.Nome)
                        .Select(x => new Usuario()
                        {
                            Nome = x
                        })
                        .ToList();
                    }

                    if (up.IdPerfil == 4)
                    {
                        ListEntity = context.Usuario
                        .Join(context.UsuarioPerfil, u => (Int64?)(u.IdUsuario), up => up.IdUsuario, (u, up) => new { u = u, up = up })
                        .Where(temp0 => ((Int32?)(temp0.up.IdPerfil) != (Int32?)1) && ((Int32?)(temp0.up.IdPerfil) != (Int32?)2) && ((Int32?)(temp0.up.IdPerfil) != (Int32?)3))
                        .Select(temp0 => temp0.u.Nome)
                        .Select(x => new Usuario()
                        {
                            Nome = x
                        })
                        .ToList();
                    }
                    if (up.IdPerfil == 6)
                    {
                        ListEntity = context.Usuario
                        .Join(context.UsuarioPerfil, u => (Int64?)(u.IdUsuario), up => up.IdUsuario, (u, up) => new { u = u, up = up })
                        .Where(temp0 => ((Int32?)(temp0.up.IdPerfil) != (Int32?)1) && ((Int32?)(temp0.up.IdPerfil) != (Int32?)2) && ((Int32?)(temp0.up.IdPerfil) != (Int32?)3))
                        .Select(temp0 => temp0.u.Nome)
                        .Select(x => new Usuario()
                        {
                            Nome = x
                        })
                        .ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;

        }
        private static string GetNavigationPerfil(ICollection<UsuarioPerfil> CollUsuarioPerfil)
        {
            string Descricao = null;
            foreach (var Item in CollUsuarioPerfil)
            {
                Descricao = Item.IdPerfilNavigation.Descricao;
            }
            return Descricao;
        }
        #endregion

        #region Outros Métodos
        public static Usuario Autenticar(string LoginEmail, string LoginSenha, string Ip)
        {
            Usuario EntityUsuario = null;
            using (YiaContext context = new YiaContext())
            {
                EntityUsuario = context.Usuario
                    .Include(y => y.UsuarioPerfil)
                    .Include(y => y.IdCorretoraNavigation)
                    .Where(x => x.Login == LoginEmail && x.Senha == Criptography.Encrypt(LoginSenha)).FirstOrDefault();
            }

            return EntityUsuario;

        }

        public static byte? GetPerfil(long IdUsuario)
        {
            byte? IdPerfil = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    IdPerfil = context.Usuario
                        .Include(y => y.UsuarioPerfil)
                            .ThenInclude(p => p.IdPerfilNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdUsuario == IdUsuario)
                        ?.FirstOrDefault()?.UsuarioPerfil.Select(x => new { IdPerfil = x.IdPerfil })?.FirstOrDefault()?.IdPerfil;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IdPerfil;
        }

        public static List<Usuario> GetAll(string parametro)
        {
            List<Usuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Usuario
                        .Where(x => x.Nome.Contains(parametro))
                        .Select(x => new Usuario()
                        {
                            IdUsuario = x.IdUsuario,
                            Nome = x.Nome,
                            CaminhoFoto = x.CaminhoFoto,
                            NomeFoto = x.NomeFoto,
                            Logado = x.Logado
                        })
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void AtualizarStatusUsuario(long IdUsuario, string status)
        {
            try
            {

                using (YiaContext context = new YiaContext())
                {
                    var usuarioStatus = context.Usuario
                   .Where(x => x.IdUsuario == IdUsuario && x.Ativo == true).First();

                    usuarioStatus.Logado = status;

                    context.Usuario.Attach(usuarioStatus);
                    context.Entry(usuarioStatus).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ObterStatusUsuario(long IdUsuario)
        {
            string status = "";
            try
            {

                using (YiaContext context = new YiaContext())
                {
                    status = context.Usuario
                   .Where(x => x.IdUsuario == IdUsuario && x.Ativo == true).Select(x => x.Logado).First();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        #endregion
    }
}