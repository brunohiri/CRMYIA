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
        public static List<List<Proposta>> GetListListCardFasesProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim, int Fase, int Salto)
        {
            List<List<Proposta>> ListEntity = new List<List<Proposta>>();
            byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);

            try
            {
                using (YiaContext context = new YiaContext())
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
                                && x.IdFaseProposta == Fase
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
                         }).Take(20).Skip(Salto).ToList());
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
                        .Where(x => x.Ativo && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
                                && x.DataSolicitacao.Value >= DataInicio
                                && x.DataSolicitacao.Value <= DataFim
                                && x.IdFaseProposta == Fase
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
                        }).Take(20).Skip(Salto).ToList());
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
                            && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario || x.IdUsuario != IdUsuario)
                            && x.IdFaseProposta == Fase
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
                        }).Take(20).Skip(Salto).ToList());
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
                            && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioMasterNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
                            && x.IdFaseProposta == Fase
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
                        }).Take(20).Skip(Salto).ToList());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        public static List<List<Proposta>> GetListListCardProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim)
        {
            List<List<Proposta>> ListEntity = new List<List<Proposta>>();
            List<FaseProposta> faseProposta = null;
            byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);

            try
            {
                using (YiaContext context = new YiaContext())
                {
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
                             }).Take(20).ToList());
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
                            .Where(x => x.Ativo && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
                                    && x.DataSolicitacao.Value >= DataInicio
                                    && x.DataSolicitacao.Value <= DataFim
                                    && x.IdFaseProposta == item.IdFaseProposta
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
                            }).Take(20).ToList());
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
                                && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario || x.IdUsuario != IdUsuario)
                                && x.IdFaseProposta == item.IdFaseProposta
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
                            }).Take(20).ToList());
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
                                && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioMasterNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
                                && x.IdFaseProposta == item.IdFaseProposta
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
                            }).Take(20).ToList());
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
        public static List<Proposta> GetListCardProposta(long IdUsuario, DateTime DataInicio, DateTime DataFim)
        {
            List<Proposta> ListEntity = null;
            byte? IdPerfil = UsuarioModel.GetPerfil(IdUsuario);
            try
            {
                using (YiaContext context = new YiaContext())
                {
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
                    else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Supervisor))
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
                    else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Administrador))
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
                            && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario || x.IdUsuario != IdUsuario)
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
                    else if (IdPerfil == (byte?)(EnumeradorModel.Perfil.Gerente))
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
                            && (x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioMasterNavigation.Where(t => t.IdUsuarioMaster == IdUsuario).Count() > 0) || (x.IdUsuario == IdUsuario)
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

        public static List<Proposta> Pesquisa(string? Nome = "", string? Descricao = "", DateTime? DataFinal = null, DateTime? DataInicial = null, long? IdUsuario = null)
        {
            List<Proposta> ListEntity = new List<Proposta>();
            try
            {

                byte? IdPerfil = UsuarioModel.GetPerfil((long)IdUsuario);
                List<Proposta> listProposta = GetListCardProposta((long)IdUsuario, DataInicial.Value, DataFinal.Value);

                foreach (Proposta item in listProposta)
                {

                    if (Nome != null || Descricao != null)
                    {
                        if (Nome != null)
                        {
                            if (item.IdUsuarioCorretorNavigation != null)
                                if ((item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))
                                {
                                    ListEntity.Add(item);
                                }
                        }
                        else if (Descricao != null)
                        {
                            if (item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation != null)
                                if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao))))
                                {
                                    ListEntity.Add(item);
                                }
                        }
                        else if (Nome != null && Descricao != null)
                        {
                            if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation != null))
                       || (item.IdUsuarioCorretorNavigation != null))
                                if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)))
                        || (item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))

                                {
                                    ListEntity.Add(item);
                                }

                        }
                    }
                    else
                    {
                        ListEntity.Add(item);
                    }


                    //else if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao))) || (item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))
                    //{
                    //    ListEntity.Add(item);
                    //}

                    //if (!Data && item.DataSolicitacao.Value == DateTime.MinValue)
                    //{
                    //    ListEntity.Add(item);
                    //}
                    //else
                    //{
                    //    if (Data)
                    //    {
                    //        if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao)))
                    //           || (item.DataSolicitacao <= DataFinal && item.DataSolicitacao >= DataInicial)
                    //           || (item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))

                    //        {
                    //            ListEntity.Add(item);
                    //        }
                    //    }
                    //    else if (((item.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao.Contains(Descricao))) || (item.IdUsuarioCorretorNavigation.Nome.Contains(Nome)))
                    //    {
                    //        ListEntity.Add(item);
                    //    }

                    //}

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
