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
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class PropostaModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static Proposta Get(long IdProposta)
        {
            Proposta Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Proposta
                        .AsNoTracking()
                        .Where(x =>x.IdProposta == IdProposta)
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

        public static List<Proposta> GetList()
        {
            List<Proposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Proposta
                        .Include(y => y.IdModalidadeNavigation)
                        .Include(y => y.IdFasePropostaNavigation)
                        .Include(y => y.IdStatusPropostaNavigation)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(y => y.IdClienteNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataCadastro).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Proposta> GetListCardProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim)
        {
            List<Proposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);
                    
                    if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Corretor))
                    {
                        ListEntity = context.Proposta
                         .Include(y => y.IdModalidadeNavigation)
                         .Include(y => y.IdFasePropostaNavigation)
                         .Include(y => y.IdStatusPropostaNavigation)
                         .Include(y => y.IdUsuarioCorretorNavigation)
                         .Include(y => y.IdUsuarioNavigation)
                         .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(k => k.IdLinhaNavigation)
                                .ThenInclude(l => l.IdProdutoNavigation)
                                    .ThenInclude(m => m.IdOperadoraNavigation)
                         .Include(y => y.IdClienteNavigation)
                         .Where(x => x.Ativo && x.IdUsuarioCorretor == IdUsuario
                                && x.DataSolicitacao.Value >= DataInicio
                                && x.DataSolicitacao.Value <= DataFim
                         )
                         .AsNoTracking()
                         .AsEnumerable()
                         .OrderBy(o => o.DataCadastro)
                         .ToList()
                         .Select(s => new Proposta()
                         {
                             IdProposta = s.IdProposta,
                             IdClienteNavigation = s.IdClienteNavigation,
                             IdFaseProposta = s.IdFaseProposta,
                             IdFasePropostaNavigation = s.IdFasePropostaNavigation,
                             IdUsuarioCorretor = s.IdUsuarioCorretor,
                             IdUsuarioCorretorNavigation = s.IdUsuarioCorretorNavigation,
                             IdCategoriaNavigation = s.IdCategoriaNavigation,//.IdLinhaNavigation.IdProdutoNavigation,
                             DataCadastro = s.DataCadastro,
                             ValorPrevisto = s.ValorPrevisto,
                             QuantidadeVidas = s.QuantidadeVidas,
                             ProximoContatoComCliente = s.ProximoContatoComCliente
                         }).ToList();
                    }
                    else
                        if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Supervisor))
                    {
                        ListEntity = context.Proposta
                        .Include(y => y.IdModalidadeNavigation)
                        .Include(y => y.IdFasePropostaNavigation)
                        .Include(y => y.IdStatusPropostaNavigation)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(t => t.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(k => k.IdLinhaNavigation)
                                .ThenInclude(l => l.IdProdutoNavigation)
                                    .ThenInclude(m => m.IdOperadoraNavigation)
                        .Include(y => y.IdClienteNavigation)
                        .Where(x => x.Ativo && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
                                && x.DataSolicitacao.Value >= DataInicio
                                && x.DataSolicitacao.Value <= DataFim
                         )
                         .AsNoTracking()
                         .AsEnumerable()
                        .OrderBy(o => o.DataCadastro)
                        .ToList()
                        .Select(s => new Proposta()
                        {
                            IdProposta = s.IdProposta,
                            IdClienteNavigation = s.IdClienteNavigation,
                            IdFaseProposta = s.IdFaseProposta,
                            IdFasePropostaNavigation = s.IdFasePropostaNavigation,
                            IdUsuarioCorretor = s.IdUsuarioCorretor,
                            IdUsuarioCorretorNavigation = s.IdUsuarioCorretorNavigation,
                            IdCategoriaNavigation = s.IdCategoriaNavigation,//.IdLinhaNavigation.IdProdutoNavigation,
                            DataCadastro = s.DataCadastro,
                            ValorPrevisto = s.ValorPrevisto,
                            QuantidadeVidas = s.QuantidadeVidas,
                            ProximoContatoComCliente = s.ProximoContatoComCliente
                        }).ToList();
                    }
                    else
                        if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Administrador))
                    {
                        ListEntity = context.Proposta
                        .Include(y => y.IdModalidadeNavigation)
                        .Include(y => y.IdFasePropostaNavigation)
                        .Include(y => y.IdStatusPropostaNavigation)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(k => k.IdLinhaNavigation)
                                .ThenInclude(l => l.IdProdutoNavigation)
                                    .ThenInclude(m => m.IdOperadoraNavigation)
                        .Include(y => y.IdClienteNavigation)
                        .Where(x => x.Ativo 
                            && x.DataSolicitacao.Value >= DataInicio
                            && x.DataSolicitacao.Value <= DataFim
                         )
                        .AsNoTracking()
                        .AsEnumerable()
                        .OrderBy(o => o.DataCadastro)
                        .ToList()
                        .Select(s => new Proposta()
                        {
                            IdProposta = s.IdProposta,
                            IdClienteNavigation = s.IdClienteNavigation,
                            IdFaseProposta = s.IdFaseProposta,
                            IdFasePropostaNavigation = s.IdFasePropostaNavigation,
                            IdUsuarioCorretor = s.IdUsuarioCorretor,
                            IdUsuarioCorretorNavigation = s.IdUsuarioCorretorNavigation,
                            IdCategoriaNavigation = s.IdCategoriaNavigation,//.IdLinhaNavigation.IdProdutoNavigation,
                            DataCadastro = s.DataCadastro,
                            ValorPrevisto = s.ValorPrevisto,
                            QuantidadeVidas = s.QuantidadeVidas,
                            ProximoContatoComCliente = s.ProximoContatoComCliente
                        }).ToList();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(Proposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Proposta.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Proposta Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Proposta.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Proposta> Pesquisa(string? Nome = null, string? Descricao = null, DateTime? DataFinal = null, DateTime? DataInicial = null, long? IdUsuario = null)
        {
            List<Proposta> ListEntity = new List<Proposta>();
            try
            {
                
                    byte? IdPerfil = UsuarioModel.GetPerfil((long)IdUsuario);
                    List<Proposta> listProposta = GetListCardProposta((long)IdUsuario, DataInicial.Value, DataFinal.Value);

                foreach (Proposta item in listProposta)
                {
                   
                    if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)))
                       || (item.DataSolicitacao <= DataFinal && item.DataSolicitacao >= DataInicial) 
                       && (item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))

                    {
                        ListEntity.Add(item);
                    }
                }
               

            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        #endregion
    }
}
