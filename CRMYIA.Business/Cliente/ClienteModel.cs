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
                            Documento = c.CPF,
                            Nome = c.Nome,
                            DataNascAbertura = c.DataNascimento.HasValue ? c.DataNascimento.Value.ToString("dd/MM/yyyy") : string.Empty,
                            Situacao = "Regular",
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

        public static List<Cliente> GetList()
        {
            List<Cliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Cliente
                        .Include(y => y.IdCidadeNavigation)
                            .ThenInclude(z => z.IdEstadoNavigation)
                        .Include(y => y.IdOrigemNavigation)
                        .Include(y => y.IdTipoLeadNavigation)
                        .Include(y => y.IdEstadoCivilNavigation)
                        .Include(y => y.IdGeneroNavigation)
                        .Include(y => y.UsuarioCliente)
                            .ThenInclude(z => z.IdUsuarioNavigation)
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
        #endregion
    }
}
