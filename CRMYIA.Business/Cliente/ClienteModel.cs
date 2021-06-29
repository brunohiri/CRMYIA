using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class ClienteModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Cliente Get(long IdCliente)
        {
            Cliente Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(y => y.IdOrigemNavigation)
                        .Include(y => y.IdTipoLeadNavigation)
                        .Include(y => y.IdEstadoCivilNavigation)
                        .Include(y => y.IdGeneroNavigation)
                        .Include(t => t.Telefone)
                        .Include(e => e.Email)
                        .Where(x => x.IdCliente == IdCliente)
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

        public static Cliente GetLastId()
        {
            Cliente Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .ToList()
                        .LastOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static ClienteViewModel GetWithCidadeEstadoTelefoneEmail(long IdCliente)
        {
            ClienteViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(t => t.Telefone)
                        .Include(e => e.Email)
                        .Where(x => x.IdCliente == IdCliente)
                        .AsNoTracking()
                        .Select(c => new ClienteViewModel()
                        {
                            IdCliente = c.IdCliente,
                            Nome = c.Nome,
                            Celular = (c.Telefone.Where(ct => ct.Ativo && ct.WhatsApp).Count() > 0 ? c.Telefone.Where(ct => ct.Ativo && ct.WhatsApp).OrderByDescending(o => o.DataCadastro).Select(ct => new { Celular = ct.DDD + ct.Telefone1 }).FirstOrDefault().Celular : string.Empty),
                            Telefone = (c.Telefone.Where(ct => ct.Ativo && !ct.WhatsApp).Count() > 0 ? c.Telefone.Where(ct => ct.Ativo && !ct.WhatsApp).OrderByDescending(o => o.DataCadastro).Select(ct => new { Telefone = ct.DDD + ct.Telefone1 }).FirstOrDefault().Telefone : string.Empty),
                            Email = (c.Email.Where(ct => ct.Ativo).Count() > 0 ? c.Email.Where(et => et.Ativo).OrderByDescending(o => o.DataCadastro).Select(et => new { Email = et.EmailConta }).FirstOrDefault().Email : string.Empty),
                        }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static ClienteViewModel GetWithCidadeEstadoTelefoneEmailEndereco(long? IdCliente = null, string Documento = null)
        {
            ClienteViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(t => t.Telefone)
                        .Include(e => e.Email)
                        .Include(cid => cid.IdCidadeNavigation)
                            .ThenInclude(uf => uf.IdEstadoNavigation)
                        .Where(x => (!Documento.IsNullOrEmpty() ? x.CPF == Documento : x.IdCliente == IdCliente))
                        .AsNoTracking()
                        .Select(c => new ClienteViewModel()
                        {
                            IdCliente = c.IdCliente,
                            IdClienteCriptografado = HttpUtility.UrlEncode(Criptography.Encrypt(c.IdCliente.ToString())),
                            Documento = c.CPF,
                            Nome = c.Nome,
                            DataNascAbertura = c.DataNascimento.HasValue ? c.DataNascimento.Value.ToString("dd/MM/yyyy") : string.Empty,
                            Situacao = c.SituacaoCadastral ?? "REGULAR",
                            Celular = (c.Telefone.Where(ct => ct.Ativo && ct.WhatsApp).Count() > 0 ? c.Telefone.Where(ct => ct.Ativo && ct.WhatsApp).OrderByDescending(o => o.DataCadastro).Select(ct => new { Celular = ct.DDD + ct.Telefone1 }).FirstOrDefault().Celular : string.Empty),
                            Telefone = (c.Telefone.Where(ct => ct.Ativo && !ct.WhatsApp).Count() > 0 ? c.Telefone.Where(ct => ct.Ativo && !ct.WhatsApp).OrderByDescending(o => o.DataCadastro).Select(ct => new { Telefone = ct.DDD + ct.Telefone1 }).FirstOrDefault().Telefone : string.Empty),
                            Email = (c.Email.Where(ct => ct.Ativo).Count() > 0 ? c.Email.Where(et => et.Ativo).OrderByDescending(o => o.DataCadastro).Select(et => new { Email = et.EmailConta }).FirstOrDefault().Email : string.Empty),
                            Endereco = Util.Util.RetornarEnderecoCompleto(c.Endereco, c.Numero, c.Complemento, c.Bairro, c.IdCidadeNavigation.Descricao, c.IdCidadeNavigation.IdEstadoNavigation.Sigla, c.CEP)
                        }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static Cliente GetByCPF(string Cpf = null)
        {
            Cliente Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Cliente
                        .Where(x => x.CPF == Cpf)
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

        public static List<ListaClienteViewModel> GetList(bool? StatusPlanoLead, long? IdOrigem, string? Nome, string? NomeCidade, DateTime? DataInicio, DateTime? DataFim)
        {
            List<ListaClienteViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    if(StatusPlanoLead == null && IdOrigem == null && Nome == null && NomeCidade == null)
                    {
                        ListEntity = context.Cliente
                       .Include(y => y.IdCidadeNavigation)
                           .ThenInclude(z => z.IdEstadoNavigation)
                       .Include(y => y.IdOrigemNavigation)
                       .Include(y => y.IdTipoLeadNavigation)
                       .Include(y => y.IdEstadoCivilNavigation)
                       .Include(y => y.IdGeneroNavigation)
                       .Include(y => y.UsuarioCliente)
                           .ThenInclude(y => y.IdUsuarioNavigation)
                       .AsNoTracking()
                       .Where(x => (x.DataAdesaoLead >= (DataInicio.HasValue ? DataInicio.Value : DateTime.MinValue) && x.DataAdesaoLead <= (DataFim.HasValue ? DataFim.Value : DateTime.MaxValue)))
                       .Select(x => new ListaClienteViewModel()
                       {
                           IdCliente = x.IdCliente,
                           IdClienteString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdCliente.ToString())),
                           Nome = x.Nome,
                           OrigemDescricao = x.IdOrigemNavigation.Descricao,
                           TipoLeadDescricao = x.IdTipoLeadNavigation.Descricao,
                            CorretorNome = (x.UsuarioCliente == null || x.UsuarioCliente.Count == 0 && x.UsuarioCliente.FirstOrDefault().IdUsuarioNavigation == null) ? "" : x.UsuarioCliente.FirstOrDefault().IdUsuarioNavigation.Nome,
                           //CorretorNome = GetNavigation(x.UsuarioCliente) != null ? GetNavigation(x.UsuarioCliente) : "",
                           CidadeNome = x.IdCidadeNavigation == null ? string.Empty : string.Format("{0}-{1}", x.IdCidadeNavigation.Descricao, x.IdCidadeNavigation.IdEstadoNavigation.Sigla),
                           DataCadastro = x.DataCadastro,
                           Ativo = x.Ativo,
                           QtdTelefone = x.Telefone != null ? x.Telefone.Count() : 0,
                           QtdEmail = x.Email != null ? x.Email.Count() : 0
                       })
                       .Take(1000)
                       .ToList();
                    }
                    else { 
                    ListEntity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(y => y.IdOrigemNavigation)
                        .Include(y => y.IdTipoLeadNavigation)
                        .Include(y => y.IdEstadoCivilNavigation)
                        .Include(y => y.IdGeneroNavigation)
                        .Include(y => y.UsuarioCliente)
                            .ThenInclude(y => y.IdUsuarioNavigation)
                        .AsNoTracking()
                        .Where(x => x.StatusPlanoLead == StatusPlanoLead || x.IdOrigem == IdOrigem || x.Nome.Contains(Nome) || x.IdCidadeNavigation.Descricao.Contains(NomeCidade) || (x.DataAdesaoLead >= DataInicio && x.DataAdesaoLead <= DataFim))
                        .Select(x => new ListaClienteViewModel()
                        {
                            IdCliente = x.IdCliente,
                            IdClienteString = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdCliente.ToString())),
                            Nome = x.Nome,
                            OrigemDescricao = x.IdOrigemNavigation.Descricao,
                            TipoLeadDescricao = x.IdTipoLeadNavigation.Descricao,
                            //CorretorNome = x.UsuarioCliente == null || x.UsuarioCliente.Count == 0 ? "" : x.UsuarioCliente.FirstOrDefault().IdUsuarioNavigation.Nome,
                            CorretorNome = GetNavigation(x.UsuarioCliente) != null ? GetNavigation(x.UsuarioCliente) : "",
                            CidadeNome = x.IdCidadeNavigation == null ? string.Empty : string.Format("{0}-{1}", x.IdCidadeNavigation.Descricao, x.IdCidadeNavigation.IdEstadoNavigation.Sigla),
                            DataCadastro = x.DataCadastro,
                            Ativo = x.Ativo
                        })
                        .ToList();}

                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Cliente> GetListIdNome()
        {
            List<Cliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cliente
                        .Where(x => x.Ativo)
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

        public static List<Cliente> GetListWithCidadeEstadoTelefoneEmail()
        {
            List<Cliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(z => z.Telefone)
                        .Include(z => z.Email)
                        .Where(x => x.Ativo)
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

        public static List<Cliente> GetList()
        {
            List<Cliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cliente
                        .OrderBy(x => x.Nome)
                        .Select(x => new Cliente() { 
                            IdCliente = x.IdCliente,
                            Nome = x.Nome
                        })
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

        public static void Add(Cliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cliente.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Cliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Cliente.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetNavigation(ICollection<UsuarioCliente> UsuarioCliente)
        {
            string Nome = null;
            foreach (var Item in UsuarioCliente)
            {
                Nome = Item.IdUsuarioNavigation.Nome;
            }
            return Nome;
        }
        #endregion
    }
}
