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
                        .Where(x => x.IdProposta == IdProposta)
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

        public static List<ListaPropostaViewModel> GetList(long IdUsuario)
        {
            List<ListaPropostaViewModel> ListEntity = null;
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
                        .Include(y => y.HistoricoProposta)
                         .ThenInclude(y => y.IdUsuarioNavigation)
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdUsuario == IdUsuario && x.HistoricoProposta != null)
                        .AsNoTracking()
                        .OrderByDescending(o => o.DataCadastro)
                        .Select(x => new ListaPropostaViewModel()
                        {
                            IdProposta = x.IdProposta,
                            DescricaoModalidade = x.IdModalidadeNavigation.Descricao,
                            NomeCliente = x.IdClienteNavigation.Nome,
                            NomeCorretor = x.IdUsuarioCorretorNavigation.Nome,
                            DescricaoFaseProposta = x.IdFasePropostaNavigation.Descricao,
                            ValorPrevisto = x.ValorPrevisto,
                            NomeUsuario = x.IdUsuarioNavigation.Nome,
                            DataCadastro = x.DataCadastro,
                            DescricaoStatusProposta = x.IdStatusPropostaNavigation.Descricao,
                            NomeHistoricoProposta = x.HistoricoProposta.OrderByDescending(y => y.DataCadastro).FirstOrDefault().IdUsuarioNavigation.Nome,
                            UsuarioMasterSlave = x.IdUsuarioNavigation.HistoricoProposta.OrderByDescending(y => y.DataCadastro).FirstOrDefault().UsuarioMasterSlave,
                            Cor = x.IdFasePropostaNavigation.CorSecundaria
                        }).Take(20)
                        .ToList();
                    /*
                     <tr class="@classTr">
                        <td>@(Item.IdModalidadeNavigation.Descricao.Length > 30 ? Item.IdModalidadeNavigation.Descricao.Substring(0,30) + "..." : Item.IdModalidadeNavigation.Descricao)</td>
                        <td>@(Item.IdClienteNavigation.Nome.Length > 30 ? Item.IdClienteNavigation.Nome.Substring(0,30) + "..." : Item.IdClienteNavigation.Nome)</td>
                        <td>@(Item.IdUsuarioCorretorNavigation == null ? "":Item.IdUsuarioCorretorNavigation.Nome.Length > 30 ? Item.IdUsuarioCorretorNavigation.Nome.Substring(0,30) + "..." : Item.IdUsuarioCorretorNavigation.Nome)</td>
                        <td>@(Item.IdFasePropostaNavigation.Descricao.Length > 30 ? Item.IdFasePropostaNavigation.Descricao.Substring(0,30) + "..." : Item.IdFasePropostaNavigation.Descricao)</td>
                        <td style="text-align:right;">@(string.Format("{0:c2}", Convert.ToDecimal(Item.ValorPrevisto)))</td>
                        <td>@(Item.IdUsuarioNavigation.Nome.Length > 30 ? Item.IdUsuarioNavigation.Nome.Substring(0,30) + "..." : Item.IdUsuarioNavigation.Nome)</td>
                        <td>@Item.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss")</td>
                        <td>@(Item.IdStatusPropostaNavigation.Descricao.Length > 30 ? Item.IdStatusPropostaNavigation.Descricao.Substring(0,30) + "..." : Item.IdStatusPropostaNavigation.Descricao)</td>
                        <td>@(Item.HistoricoProposta.Where(x => x.UsuarioMasterSlave == false && Item.IdProposta == x.IdProposta))</td>
                        <td>
                            <a asp-page="/NovaProposta" asp-route-id="@System.Web.HttpUtility.UrlEncode(Criptography.Encrypt(Item.IdProposta.ToString()))" title="Editar" class="text-info"><i class="icon fas fa-edit"></i></a>
                        </td>
                    </tr>
                     */
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Proposta> GetList(long IdUsuario, DateTime DataInicio, DateTime DataFim)
        {
            List<Proposta> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Proposta
                        .Include(y => y.IdModalidadeNavigation)
                        .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(h => h.IdLinhaNavigation)
                                .ThenInclude(h => h.IdProdutoNavigation)
                                    .ThenInclude(h => h.IdOperadoraNavigation)
                        .Include(y => y.IdFasePropostaNavigation)
                        .Include(y => y.IdStatusPropostaNavigation)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                        .Include(y => y.IdUsuarioNavigation)
                        .Include(y => y.IdClienteNavigation)
                            .ThenInclude(h => h.IdEstadoCivilNavigation)
                        .Include(y => y.IdClienteNavigation)
                            .ThenInclude(h => h.IdGeneroNavigation)
                        .Include(y => y.IdClienteNavigation)
                            .ThenInclude(h => h.IdCidadeNavigation)
                                .ThenInclude(h => h.IdEstadoNavigation)
                        .Include(y => y.HistoricoProposta)
                         .ThenInclude(y => y.IdUsuarioNavigation)
                        .Where(x => x.Ativo && x.IdUsuario == IdUsuario && x.HistoricoProposta != null
                                    && x.DataSolicitacao.Value >= DataInicio
                                    && x.DataSolicitacao.Value <= DataFim)
                        .OrderByDescending(o => o.DataCadastro)
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


        /// <summary> Recupera as informações necessárias das propostas para cada fase do funil de venda de forma assíncrona, respeitando a hierarquia dos usuários que fazem as requisições. </summary>
        /// <param name="idUsuario">ID do usuário logado ou do usuário definido no formulário de pesquisa</param>
        /// <param name="dataInicio">Datetime inicial</param>
        /// <param name="dataFim">Datetime final</param>
        /// <param name="listFaseProposta">Uma lista com as fases do funil de venda</param>
        /// <param name="operadora">[Opcional] Inclui a operadora na requisição</param>
        /// <param name="corretor">[Opcional] Inclui o corretor na requisição</param>
        /// <param name="salto">[Opcional] Inclui um salto na requisição para a paginação</param>
        /// <exception cref="AggregateException"></exception>
        /// <returns>Retorna uma Task que será resolvida quando todas as requests forem concluídas</returns>
        public static async Task<List<List<Proposta>>> GetListCardPropostaAsync(long idUsuario, DateTime dataInicio, DateTime dataFim, List<FaseProposta> listFaseProposta, string operadora = "", string corretor = "", int salto = 0)
        {
            List<List<Proposta>> listPropostas = new List<List<Proposta>>();
            List<Task<List<Proposta>>> taskPropostas = new List<Task<List<Proposta>>>();

            foreach (FaseProposta faseProposta in listFaseProposta)
            {
                taskPropostas.Add(GetPropostaListAsync(idUsuario, dataInicio, dataFim, faseProposta.IdFaseProposta, salto, operadora));
            }

            Task t = Task.WhenAll(taskPropostas.ToArray());

            try
            {
                await t;
            } catch (AggregateException) { }

            if (t.Status == TaskStatus.RanToCompletion)
            {
                foreach (var propostas in taskPropostas)
                {
                    if (propostas.Status == TaskStatus.RanToCompletion)
                    {
                        listPropostas.Add(propostas.Result);
                    }
                }
            }

            return listPropostas;
        }

        /// <summary> Recupera as informações necessárias das propostas para uma fase do funil de venda de forma assíncrona, respeitando a hierarquia dos usuários que fazem as requisições. </summary>
        /// <param name="idUsuario">ID do usuário logado ou do usuário definido no formulário de pesquisa</param>
        /// <param name="dataInicio">Datetime inicial</param>
        /// <param name="dataFim">Datetime final</param>
        /// <param name="fase">ID da fase atual da proposta</param>
        /// <param name="salto">[Opcional] Inclui um salto na requisição para a paginação</param>
        /// <exception cref="Exception"></exception>
        /// <returns>Retorna uma Task para a fase atual da proposta</returns>
        private static async Task<List<Proposta>> GetPropostaListAsync(long idUsuario, DateTime dataInicio, DateTime dataFim, byte fase, int salto = 0, string operadora = "")
        {
            List<Proposta> taskPropostas = new List<Proposta>();

            try
            {
                using (YiaContext context = new YiaContext())
                {
                    taskPropostas = await context.Proposta
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
                        .Where(x => x.Ativo
                            && x.DataSolicitacao.Value >= dataInicio
                            && x.DataSolicitacao.Value <= dataFim
                            && x.IdFaseProposta == fase
                            && x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(operadora)
                            && ((x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == idUsuario).Count() > 0) || x.IdUsuario == idUsuario || x.IdUsuarioCorretor == idUsuario))
                        .OrderBy(o => o.DataCadastro)
                        .AsNoTracking().Skip(salto).Take(20).ToListAsync();
                }

                return taskPropostas;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<List<Proposta>> GetListListCardProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim, string Descricao, string Nome, byte Fase, int Salto)
        {
            List<List<Proposta>> ListEntity = new List<List<Proposta>>();
            List<FaseProposta> faseProposta = null;
            byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);

            try
            {
                using (YiaContext context = new YiaContext())
                {
                    if (Fase > 0)
                        faseProposta = context.FaseProposta.Where(x => x.IdFaseProposta == Fase).ToList();
                    else
                        faseProposta = context.FaseProposta.ToList();

                    foreach (var item in faseProposta)
                    {
                        if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Corretor))
                        {
                            ListEntity.Add(context.Proposta
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
                                    && x.IdFaseProposta == item.IdFaseProposta
                                    && x.IdUsuarioCorretorNavigation.Nome.Contains(Nome)
                                    && x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)
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
                             }).Skip(Salto).Take(20).ToList());
                        }
                        else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Supervisor))
                        {
                            ListEntity.Add(context.Proposta
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
                            .Where(x => x.Ativo && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0 || (x.IdUsuario == IdUsuario))
                                    && x.DataSolicitacao.Value >= DataInicio
                                    && x.DataSolicitacao.Value <= DataFim
                                    && x.IdFaseProposta == item.IdFaseProposta
                                    && x.IdUsuarioCorretorNavigation.Nome.Contains(Nome)
                                    && x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)
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
                            }).Skip(Salto).Take(20).ToList());
                        }
                        else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Administrador))
                        {
                            ListEntity.Add(context.Proposta
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
                                && ((x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || x.IdUsuario == IdUsuario)
                                && x.IdFaseProposta == item.IdFaseProposta
                                && x.IdUsuarioCorretorNavigation.Nome.Contains(Nome)
                                && x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)
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
                                DataSolicitacao = s.DataSolicitacao,
                                ValorPrevisto = s.ValorPrevisto,
                                QuantidadeVidas = s.QuantidadeVidas,
                                ProximoContatoComCliente = s.ProximoContatoComCliente
                            }).Skip(Salto).Take(20).ToList());
                        }
                        else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Gerente))
                        {
                            ListEntity.Add(context.Proposta
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
                                && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioMasterNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0 || (x.IdUsuario == IdUsuario))
                                && x.IdFaseProposta == item.IdFaseProposta
                                && x.IdUsuarioCorretorNavigation.Nome.Contains(Nome)
                                && x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)
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
                            }).Skip(Salto).Take(20).ToList());
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
        public static List<ListKPIRealizadoPropostaViewModel> GetListKPIRealizadoProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim)
        {
            List<ListKPIRealizadoPropostaViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);

                    ListEntity = context.Proposta
                        .Where(x => x.Ativo && x.IdUsuarioCorretor == IdUsuario
                               && x.DataSolicitacao.Value >= DataInicio
                               && x.DataSolicitacao.Value <= DataFim
                        )
                        .AsNoTracking()
                        .ToList()
                        .Select(s => new ListKPIRealizadoPropostaViewModel()
                        {
                            IdUsuario = (long)s.IdUsuarioCorretor,
                            DataSolicitacao = s.DataSolicitacao,
                            ValorPrevisto = s.ValorPrevisto,
                            QuantidadeVidas = s.QuantidadeVidas
                        }).ToList();
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


        #endregion
    }
}
