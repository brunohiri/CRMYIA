using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Web;

namespace CRMYIA.Business.Dashboard
{
    public class DashboardViewModel
    {
        #region Propriedades

        #region Diretoria
        #region Bloco Quantitativos
        public string ValorPropostasImplantadas { get; set; }
        public string QtdOrcamentosEmNegociacao { get; set; }
        public string QtdCorretoresProduzindo { get; set; }
        public string QtdCorretoresInativos { get; set; }
        #endregion

        #region Bloco Produção
        public List<OperadoraMaisVendidaViewModel> OperadorasMaisVendidas { get; set; }
        public string ValorSegurosFechados { get; set; }
        public string ValorPlanoSaude { get; set; }
        public string ValorMetaEstipulada { get; set; }
        public string QtdNegociosPerdidos { get; set; }
        public List<ValorProducaoPorDiaViewModel> ListValorProducaoPorDia { get; set; }
        #endregion

        #region Bloco Posição no Ranking
        public byte PosicaoRanking { get; set; }
        public string CorretoresAniversariantesMes { get; set; }
        #endregion

        #region Bloco Ranking Gerentes
        public List<RankingViewModel> ListRankingGerentes { get; set; }
        #endregion

        #region Bloco Ranking Supervisores
        public List<RankingViewModel> ListRankingSupervisores { get; set; }
        #endregion

        #region Bloco Ranking Corretores
        public List<RankingViewModel> ListRankingCorretores { get; set; }
        #endregion

        #region Bloco Carteira
        public List<CarteiraViewModel> ListCarteira { get; set; }
        #endregion
        #endregion

        #region Corretores

        #region Bloco Quantitativos
        public string QtdSegurosARenovar { get; set; }
        public string QtdApolicesVigentes { get; set; }
        #endregion

        #region Bloco Posição no Ranking
        public string QuantidadeCorretoresCadastrados { get; set; }
        public string ValorInadimplencia { get; set; }
        #endregion

        #region Bloco Propostas Pendentes
        public List<PropostaViewModel> PropostasPendentes { get; set; }
        #endregion

        #endregion
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static DashboardViewModel GetQuantificadores(EnumeradorModel.Perfil TipoPerfil, long? IdUsuario = null)
        {
            DashboardViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = new DashboardViewModel();

                    if (TipoPerfil == EnumeradorModel.Perfil.Administrador)
                    {
                        #region Administrador/Diretoria
                        Entity.ValorPropostasImplantadas = context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.QtdOrcamentosEmNegociacao = string.Format("{0:#,0}", Convert.ToInt64(context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise))
                            .Count()));

                        Entity.QtdCorretoresProduzindo = string.Format("{0:#,0}", Convert.ToInt64(context.Proposta.Where(x => x.Ativo).Select(s => new { IdUsuarioCorretor = s.IdUsuarioCorretor }).Distinct().Count()));

                        Entity.QtdCorretoresInativos = string.Format("{0:#,0}", Convert.ToInt64(context.Usuario.Where(x => !x.Ativo).Count()));
                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Gerente)
                    {
                        #region Gerentes
                        Entity.ValorPropostasImplantadas = context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                         && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                        .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.QtdOrcamentosEmNegociacao = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Proposta
                             .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise)
                           && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                            .Count()));

                        Entity.QtdCorretoresProduzindo = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Proposta
                             .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo
                           && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                            .Select(s => new { IdUsuarioCorretor = s.IdUsuarioCorretor }).Distinct()
                            .Count()));

                        Entity.QtdCorretoresInativos = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Usuario
                            .Include(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                            .Where(x => !x.Ativo && x.UsuarioHierarquiaIdUsuarioMasterNavigation.FirstOrDefault().IdUsuarioMaster == IdUsuario)
                            .Count()));
                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Supervisor)
                    {
                        #region Supervisores
                        Entity.ValorPropostasImplantadas = context.Proposta
                           .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                          && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                                .Count() > 0)
                        .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.QtdOrcamentosEmNegociacao = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                                .Count() > 0)
                            .Count()));

                        Entity.QtdCorretoresProduzindo = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                                .Count() > 0)
                            .Select(s => new { IdUsuarioCorretor = s.IdUsuarioCorretor }).Distinct()
                            .Count()));

                        Entity.QtdCorretoresInativos = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Usuario
                            .Include(y => y.UsuarioHierarquiaIdUsuarioMasterNavigation)
                            .Where(x => !x.Ativo && x.UsuarioHierarquiaIdUsuarioMasterNavigation.FirstOrDefault().IdUsuarioMaster == IdUsuario)
                            .Count()));
                        #endregion
                    }
                    else
                         if ((TipoPerfil == EnumeradorModel.Perfil.Corretor) || (TipoPerfil == EnumeradorModel.Perfil.Vendedor))
                    {
                        #region Corretores/Vendedores
                        Entity.ValorPropostasImplantadas = context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito) && x.IdUsuarioCorretor == IdUsuario)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.QtdOrcamentosEmNegociacao = string.Format("{0:#,0}", Convert.ToInt64(context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise) && x.IdUsuarioCorretor == IdUsuario)
                            .Count()));

                        Entity.QtdSegurosARenovar = string.Format("{0:#,0}", Convert.ToInt64(context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito) && x.IdUsuarioCorretor == IdUsuario && x.DataCadastro.AddYears(1) >= DateTime.Now)
                            .Count()));

                        Entity.QtdApolicesVigentes = string.Format("{0:#,0}", Convert.ToInt64(context.Proposta.Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito) && x.IdUsuarioCorretor == IdUsuario)
                            .Count()));
                        #endregion

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static DashboardViewModel GetProducao(EnumeradorModel.Perfil TipoPerfil, long? IdUsuario = null)
        {
            DashboardViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    DateTime DataInicial = Util.Util.GetFirstDayOfMonth(DateTime.Now.Month - 1);
                    DateTime DataFinal = Util.Util.GetLastDayOfMonth(DateTime.Now.Month - 1);
                    Entity = new DashboardViewModel();

                    if (TipoPerfil == EnumeradorModel.Perfil.Administrador)
                    {
                        #region Administrador/Diretoria
                        Entity.OperadorasMaisVendidas = context.Proposta
                        .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(z => z.IdLinhaNavigation)
                                .ThenInclude(k => k.IdProdutoNavigation)
                                    .ThenInclude(p => p.IdOperadoraNavigation)
                         .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                        && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                        && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .AsEnumerable()
                        .GroupBy(g => new
                        {
                            IdOperadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.IdOperadora,
                            Descricao = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao
                        })
                        .Select(s => new OperadoraMaisVendidaViewModel()
                        {
                            IdOperadora = s.Key.IdOperadora,
                            Descricao = s.Key.Descricao,
                            Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2")
                        })
                        .ToList();

                        Entity.ValorSegurosFechados = context.Proposta
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorPlanoSaude = context.Proposta
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorMetaEstipulada = context.Meta
                            .Where(x => x.Ativo)
                            .Sum(y => y.ValorMaximo.Value).ToString("c2");

                        Entity.QtdNegociosPerdidos = context.Proposta
                            .Where(x => !x.Ativo || x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Declinado))
                            .Count().ExtractIntMilharFormat();

                        Entity.ListValorProducaoPorDia = context.Proposta
                            .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(z => z.IdLinhaNavigation)
                                .ThenInclude(k => k.IdProdutoNavigation)
                                    .ThenInclude(p => p.IdOperadoraNavigation)
                           .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal))
                           .AsEnumerable()
                           .GroupBy(g => new
                           {
                               Operadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                               Dia = g.DataSolicitacao.Value.Day.ToString()
                           })
                           .Select(s => new ValorProducaoPorDiaViewModel()
                           {
                               Dia = s.Key.Dia.ExtractInt32(),
                               Operadora = s.Key.Operadora,
                               Valor = s.Sum(soma => soma.ValorPrevisto).Value//.ToString("c2")
                           })
                           .OrderBy(o => o.Dia)
                           .ToList();

                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Gerente)
                    {
                        #region Gerentes
                        Entity.OperadorasMaisVendidas = context.Proposta
                         .Include(y => y.IdCategoriaNavigation)
                             .ThenInclude(z => z.IdLinhaNavigation)
                                 .ThenInclude(k => k.IdProdutoNavigation)
                                     .ThenInclude(p => p.IdOperadoraNavigation)
                         .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                          .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                           && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                           && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0
                         && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                         .GroupBy(g => new
                         {
                             IdOperadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.IdOperadora,
                             Descricao = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao
                         })
                         .Select(s => new OperadoraMaisVendidaViewModel()
                         {
                             IdOperadora = s.Key.IdOperadora,
                             Descricao = s.Key.Descricao,
                             Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2")
                         })
                         .ToList();

                        Entity.ValorSegurosFechados = context.Proposta
                           .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorPlanoSaude = context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorMetaEstipulada = context.Meta
                            .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
                            .Sum(y => y.ValorMaximo.Value).ToString("c2");

                        Entity.QtdNegociosPerdidos = context.Proposta
                          .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => !x.Ativo || x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Declinado)
                             && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0)
                            .Count().ExtractIntMilharFormat();

                        Entity.ListValorProducaoPorDia = context.Proposta
                            .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(z => z.IdLinhaNavigation)
                                .ThenInclude(k => k.IdProdutoNavigation)
                                    .ThenInclude(p => p.IdOperadoraNavigation)
                           .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                           .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                            .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0)
                                                                            .Count() > 0
                            && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal))
                           .AsEnumerable()
                           .GroupBy(g => new
                           {
                               Operadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                               Dia = g.DataSolicitacao.Value.Day.ToString()
                           })
                           .Select(s => new ValorProducaoPorDiaViewModel()
                           {
                               Dia = s.Key.Dia.ExtractInt32(),
                               Operadora = s.Key.Operadora,
                               Valor = s.Sum(soma => soma.ValorPrevisto).Value//.ToString("c2")
                           })
                           .OrderBy(o => o.Dia)
                           .ToList();

                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Supervisor)
                    {
                        #region Supervisores
                        Entity.OperadorasMaisVendidas = context.Proposta
                         .Include(y => y.IdCategoriaNavigation)
                             .ThenInclude(z => z.IdLinhaNavigation)
                                 .ThenInclude(k => k.IdProdutoNavigation)
                                     .ThenInclude(p => p.IdOperadoraNavigation)
                         .Include(y => y.IdUsuarioCorretorNavigation)
                         .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                          .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                           && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                           && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0
                         && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                         .GroupBy(g => new
                         {
                             IdOperadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.IdOperadora,
                             Descricao = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao
                         })
                         .Select(s => new OperadoraMaisVendidaViewModel()
                         {
                             IdOperadora = s.Key.IdOperadora,
                             Descricao = s.Key.Descricao,
                             Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2")
                         })
                         .ToList();

                        Entity.ValorSegurosFechados = context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                             .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                             && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorPlanoSaude = context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorMetaEstipulada = context.Meta
                            .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
                            .Sum(y => y.ValorMaximo.Value).ToString("c2");

                        Entity.QtdNegociosPerdidos = context.Proposta
                            .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                            .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => !x.Ativo || x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Declinado)
                             && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0)
                            .Count().ExtractIntMilharFormat();

                        Entity.ListValorProducaoPorDia = context.Proposta
                            .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(z => z.IdLinhaNavigation)
                                .ThenInclude(k => k.IdProdutoNavigation)
                                    .ThenInclude(p => p.IdOperadoraNavigation)
                           .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                           .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                            && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0
                            && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                            )
                           .AsEnumerable()
                           .GroupBy(g => new
                           {
                               Operadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                               Dia = g.DataSolicitacao.Value.Day.ToString()
                           })
                           .Select(s => new ValorProducaoPorDiaViewModel()
                           {
                               Dia = s.Key.Dia.ExtractInt32(),
                               Operadora = s.Key.Operadora,
                               Valor = s.Sum(soma => soma.ValorPrevisto).Value//.ToString("c2")
                                                   })
                           .OrderBy(o => o.Dia)
                           .ToList();
                        #endregion
                    }
                    else
                         if ((TipoPerfil == EnumeradorModel.Perfil.Corretor) || (TipoPerfil == EnumeradorModel.Perfil.Vendedor))
                    {
                        #region Corretores/Vendedores
                        Entity.OperadorasMaisVendidas = context.Proposta
                          .Include(y => y.IdCategoriaNavigation)
                              .ThenInclude(z => z.IdLinhaNavigation)
                                  .ThenInclude(k => k.IdProdutoNavigation)
                                      .ThenInclude(p => p.IdOperadoraNavigation)
                           .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                          && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                          && x.IdUsuarioCorretor == IdUsuario
                          && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                          .GroupBy(g => new
                          {
                              IdOperadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.IdOperadora,
                              Descricao = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao
                          })
                          .Select(s => new OperadoraMaisVendidaViewModel()
                          {
                              IdOperadora = s.Key.IdOperadora,
                              Descricao = s.Key.Descricao,
                              Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2")
                          })
                          .ToList();

                        Entity.ValorSegurosFechados = context.Proposta
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito) && x.IdUsuarioCorretor == IdUsuario)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorPlanoSaude = context.Proposta
                            .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito) && x.IdUsuarioCorretor == IdUsuario)
                            .Sum(y => y.ValorPrevisto.Value).ToString("c2");

                        Entity.ValorMetaEstipulada = context.Meta
                            .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
                            .Sum(y => y.ValorMaximo.Value).ToString("c2");

                        Entity.QtdNegociosPerdidos = context.Proposta
                            .Where(x => !x.Ativo || x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Declinado) && x.IdUsuarioCorretor == IdUsuario)
                            .Count().ExtractIntMilharFormat();

                        Entity.ListValorProducaoPorDia = context.Proposta
                          .Include(y => y.IdCategoriaNavigation)
                          .ThenInclude(z => z.IdLinhaNavigation)
                              .ThenInclude(k => k.IdProdutoNavigation)
                                  .ThenInclude(p => p.IdOperadoraNavigation)
                         .Where(x => x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito)
                          && x.IdUsuarioCorretor == IdUsuario
                          && (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                          )
                         .AsEnumerable()
                         .GroupBy(g => new
                         {
                             Operadora = g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                             Dia = g.DataSolicitacao.Value.Day.ToString()
                         })
                         .Select(s => new ValorProducaoPorDiaViewModel()
                         {
                             Dia = s.Key.Dia.ExtractInt32(),
                             Operadora = s.Key.Operadora,
                             Valor = s.Sum(soma => soma.ValorPrevisto).Value//.ToString("c2")
                           })
                         .OrderBy(o => o.Dia)
                         .ToList();
                        #endregion
                    }
                }

                #region Ordenar
                if (Entity.OperadorasMaisVendidas != null)
                {
                    int QtdTake = (Entity.OperadorasMaisVendidas.Count() < 4 ? Entity.OperadorasMaisVendidas.Count() : 4);
                    Entity.OperadorasMaisVendidas = Entity.OperadorasMaisVendidas.OrderBy(o => o.Valor).Take(QtdTake).ToList();
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static DashboardViewModel GetRankings(EnumeradorModel.Perfil TipoPerfil, long? IdUsuario = null)
        {
            DashboardViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    DateTime DataInicial = Util.Util.GetFirstDayOfMonth(DateTime.Now.Month);
                    DateTime DataFinal = Util.Util.GetLastDayOfMonth(DateTime.Now.Month);
                    Entity = new DashboardViewModel();

                    if (TipoPerfil == EnumeradorModel.Perfil.Administrador)
                    {
                        #region Gerentes
                        Entity.ListRankingGerentes = context.Proposta
                       .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                       .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                       && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0
                       //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= Da1taFinal)
                       && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                       .AsEnumerable()
                       .GroupBy(g => new
                       {
                           IdUsuario =
                                g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation == null
                                || g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation == null
                                || g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Count == 0
                                ? 0 : g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMaster,
                           Nome = g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation == null
                                || g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation == null
                                || g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.Count == 0
                                || g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation == null
                                ? "" : g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.Nome.ToString()
                       })
                       .Select(s => new RankingViewModel()
                       {
                           Posicao = s.Key.IdUsuario.ToString(),
                           Nome = s.Key.Nome,
                           Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                           NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                       })
                       .ToList();
                        #endregion

                        #region Supervisores
                        Entity.ListRankingSupervisores = context.Proposta
                        .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                        && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                .Where(h => h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                                .Count() > 0
                        //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= Da1taFinal)
                        && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .AsEnumerable()
                        .GroupBy(g => new
                        {
                            IdUsuario = g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMaster,
                            Nome = g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.Nome.ToString()
                        })
                        .Select(s => new RankingViewModel()
                        {
                            Posicao = s.Key.IdUsuario.ToString(),
                            Nome = s.Key.Nome,
                            Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                            NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                        })
                        .ToList();
                        #endregion

                        #region Corretores
                        Entity.ListRankingCorretores = context.Proposta
                        .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                        && x.IdUsuarioCorretorNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Corretor).Count() > 0
                        //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                        && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .GroupBy(g => new
                        {
                            IdUsuario = g.IdUsuarioCorretorNavigation.IdUsuario,
                            Nome = g.IdUsuarioCorretorNavigation.Nome,
                        })
                        .Select(s => new RankingViewModel()
                        {
                            Posicao = s.Key.IdUsuario.ToString(),
                            Nome = s.Key.Nome,
                            Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                            NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                        })
                        .ToList();
                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Gerente)
                    {
                        #region Ranking Supervisores
                        Entity.ListRankingSupervisores = context.Proposta
                        .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                         .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                             && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                .Count() > 0
                         //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= Da1taFinal)
                         && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                         .AsEnumerable()
                         .GroupBy(g => new
                         {
                             IdUsuario = g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMaster,
                             Nome = g.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.Nome.ToString()
                         })
                         .Select(s => new RankingViewModel()
                         {
                             Posicao = s.Key.IdUsuario.ToString(),
                             Nome = s.Key.Nome,
                             Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                             NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                         })
                         .ToList();
                        #endregion

                        #region Ranking Corretores
                        Entity.ListRankingCorretores = context.Proposta
                         .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                        && x.IdUsuarioCorretorNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Corretor).Count() > 0
                        //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                       && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0
                        && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .GroupBy(g => new
                        {
                            IdUsuario = g.IdUsuarioCorretorNavigation.IdUsuario,
                            Nome = g.IdUsuarioCorretorNavigation.Nome,
                        })
                        .Select(s => new RankingViewModel()
                        {
                            Posicao = s.Key.IdUsuario.ToString(),
                            Nome = s.Key.Nome,
                            Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                            NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                        })
                        .ToList();
                        #endregion
                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Supervisor)
                    {
                        #region Ranking Corretores
                        Entity.ListRankingCorretores = context.Proposta
                        .Include(y => y.IdUsuarioCorretorNavigation)
                            .ThenInclude(y => y.UsuarioPerfil)
                        .Include(y => y.IdUsuarioCorretorNavigation)
                        .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                        .Where(x => x.IdUsuarioCorretorNavigation.Ativo
                        && x.IdUsuarioCorretorNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Corretor).Count() > 0
                        //&& (x.DataSolicitacao.Value >= DataInicial && x.DataSolicitacao.Value <= DataFinal)
                        && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                            .Count() > 0
                        && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .GroupBy(g => new
                        {
                            IdUsuario = g.IdUsuarioCorretorNavigation.IdUsuario,
                            Nome = g.IdUsuarioCorretorNavigation.Nome,
                        })
                        .Select(s => new RankingViewModel()
                        {
                            Posicao = s.Key.IdUsuario.ToString(),
                            Nome = s.Key.Nome,
                            Valor = s.Sum(soma => soma.ValorPrevisto).Value.ToString("c2"),
                            NumeroVidas = s.Sum(soma => soma.QuantidadeVidas).Value.ExtractIntMilharFormat()
                        })
                        .ToList();
                        #endregion
                    }

                    if (Entity.ListRankingGerentes != null)
                    {
                        int QtdTake = (Entity.ListRankingGerentes.Count() < 4 ? Entity.ListRankingGerentes.Count() : 4);
                        Entity.ListRankingGerentes = Entity.ListRankingGerentes.OrderByDescending(o => o.Valor.Replace("R$ ", string.Empty).ExtractDecimal()).Take(QtdTake).ToList();
                    }

                    if (Entity.ListRankingSupervisores != null)
                    {
                        int QtdTake = (Entity.ListRankingSupervisores.Count() < 4 ? Entity.ListRankingSupervisores.Count() : 4);
                        Entity.ListRankingSupervisores = Entity.ListRankingSupervisores.OrderByDescending(o => o.Valor.Replace("R$ ", string.Empty).ExtractDecimal()).Take(QtdTake).ToList();
                    }

                    if (Entity.ListRankingCorretores != null)
                    {
                        int QtdTake = (Entity.ListRankingCorretores.Count() < 4 ? Entity.ListRankingCorretores.Count() : 4);
                        Entity.ListRankingCorretores = Entity.ListRankingCorretores.OrderByDescending(o => o.Valor.Replace("R$ ", string.Empty).ExtractDecimal()).Take(QtdTake).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static DashboardViewModel GetRankingUsuarioCorretoresAniversariantes(EnumeradorModel.Perfil TipoPerfil, long? IdUsuario = null)
        {
            DashboardViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    DateTime DataInicial = Util.Util.GetFirstDayOfMonth(DateTime.Now.Month);
                    DateTime DataFinal = Util.Util.GetLastDayOfMonth(DateTime.Now.Month);
                    Entity = new DashboardViewModel();

                    Entity.CorretoresAniversariantesMes = context.Usuario
                             .Where(x => x.DataNascimentoAbertura.HasValue ? (x.DataNascimentoAbertura.Value.Day == DateTime.Now.Day && x.DataNascimentoAbertura.Value.Month == DateTime.Now.Month) : false)
                             .Count().ExtractIntMilharFormat();

                    Entity.PosicaoRanking = 1;

                    if (TipoPerfil == EnumeradorModel.Perfil.Gerente)
                    {
                    }
                    else
                    if (TipoPerfil == EnumeradorModel.Perfil.Supervisor)
                    {
                        Entity.QuantidadeCorretoresCadastrados = string.Format("{0:#,0}", Convert.ToInt64(
                            context.Usuario
                            .Include(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                   .ThenInclude(y => y.UsuarioPerfil)
                            .Where(x => x.Ativo && x.UsuarioPerfil.Where(y => y.IdPerfil == (byte?)(EnumeradorModel.Perfil.Corretor)).Count() > 0
                             && x.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                            .Count() > 0)
                       .Distinct().Count()));
                    }
                    else
                    {
                        Entity.QuantidadeCorretoresCadastrados = string.Format("{0:#,0}", Convert.ToInt64(context.Usuario.Where(x => x.Ativo && x.UsuarioPerfil.Where(y => y.IdPerfil == (byte?)(EnumeradorModel.Perfil.Corretor)).Count() > 0)
                        .Distinct().Count()));
                    }

                    Entity.ValorInadimplencia = 0.ToString("c2");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }

        public static DashboardViewModel GetPropostasPendentes(EnumeradorModel.Perfil TipoPerfil, long? IdUsuario = null)
        {
            DashboardViewModel Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = new DashboardViewModel();

                    if (TipoPerfil == EnumeradorModel.Perfil.Gerente)
                    {

                    }
                    else
                         if (TipoPerfil == EnumeradorModel.Perfil.Supervisor)
                    {
                        #region Supervisores
                        Entity.PropostasPendentes = context.Proposta
                         .Include(y => y.IdFasePropostaNavigation)
                         .Include(y => y.IdCategoriaNavigation)
                             .ThenInclude(z => z.IdLinhaNavigation)
                                 .ThenInclude(k => k.IdProdutoNavigation)
                                     .ThenInclude(p => p.IdOperadoraNavigation)
                         .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                          .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                         && x.IdUsuarioCorretor == IdUsuario
                         && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMaster == IdUsuario && h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Supervisor).Count() > 0)
                                                                                .Count() > 0
                         && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise)
                         && (x.IdFaseProposta == (byte?)(EnumeradorModel.FaseProposta.ImplantacaoDocumentacao)
                            || x.IdFaseProposta == (byte?)(EnumeradorModel.FaseProposta.VisitaApresentacaoReuniao)))
                         .Select(s => new PropostaViewModel()
                         {
                             IdProposta = s.IdProposta.ToString(),
                             IdPropostaCript = HttpUtility.UrlEncode(Criptography.Encrypt(s.IdProposta.ToString())),
                             Operadora = s.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                             Supervisor = s.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.Nome,
                             Consideracoes = s.Observacoes,
                             DataPendencia = s.DataCadastro.ToString("dd/MM/yyyy"),
                             Status = s.IdFasePropostaNavigation.Descricao
                         })
                         .ToList();
                        #endregion
                    }
                    else
                         if ((TipoPerfil == EnumeradorModel.Perfil.Corretor) || (TipoPerfil == EnumeradorModel.Perfil.Vendedor))
                    {
                        #region Corretores/Vendedor
                        Entity.PropostasPendentes = context.Proposta
                         .Include(y => y.IdFasePropostaNavigation)
                         .Include(y => y.IdCategoriaNavigation)
                             .ThenInclude(z => z.IdLinhaNavigation)
                                 .ThenInclude(k => k.IdProdutoNavigation)
                                     .ThenInclude(p => p.IdOperadoraNavigation)
                         .Include(y => y.IdUsuarioCorretorNavigation)
                           .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                               .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                    .ThenInclude(y => y.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                                        .ThenInclude(y => y.IdUsuarioMasterNavigation)
                                            .ThenInclude(y => y.UsuarioPerfil)
                          .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                         && x.IdUsuarioCorretor == IdUsuario
                         && x.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                               .Where(a => a.IdUsuarioMasterNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation
                                                                                    .Where(h => h.IdUsuarioMasterNavigation.UsuarioPerfil.Where(p => p.IdPerfil == (byte?)EnumeradorModel.Perfil.Gerente).Count() > 0)
                                                                                    .Count() > 0)
                                                                                .Count() > 0
                         && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.EmAnalise)
                         && (x.IdFaseProposta == (byte?)(EnumeradorModel.FaseProposta.ImplantacaoDocumentacao)
                            || x.IdFaseProposta == (byte?)(EnumeradorModel.FaseProposta.VisitaApresentacaoReuniao)))
                         .Select(s => new PropostaViewModel()
                         {
                             IdProposta = s.IdProposta.ToString(),
                             IdPropostaCript = HttpUtility.UrlEncode(Criptography.Encrypt(s.IdProposta.ToString())),
                             Operadora = s.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Descricao,
                             Supervisor = s.IdUsuarioCorretorNavigation.UsuarioHierarquiaIdUsuarioSlaveNavigation.FirstOrDefault().IdUsuarioMasterNavigation.Nome,
                             Consideracoes = s.Observacoes,
                             DataPendencia = s.DataCadastro.ToString("dd/MM/yyyy"),
                             Status = s.IdFasePropostaNavigation.Descricao
                         })
                         .ToList();
                        #endregion
                    }
                }
                if (Entity.PropostasPendentes != null)
                {
                    int QtdTake = (Entity.PropostasPendentes.Count() < 4 ? Entity.PropostasPendentes.Count() : 4);
                    Entity.PropostasPendentes = Entity.PropostasPendentes.OrderByDescending(o => o.DataPendencia).Take(QtdTake).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        #endregion
    }
}
