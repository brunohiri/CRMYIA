using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CRMYIA.Data.Entities;

namespace CRMYIA.Data.Context
{
    public partial class YiaContext : DbContext
    {
        public YiaContext()
        {
        }

        public YiaContext(DbContextOptions<YiaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Abordagem> Abordagem { get; set; }
        public virtual DbSet<AbordagemCategoria> AbordagemCategoria { get; set; }
        public virtual DbSet<ArquivoLead> ArquivoLead { get; set; }
        public virtual DbSet<AssinaturaCartao> AssinaturaCartao { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<BannerOperadora> BannerOperadora { get; set; }
        public virtual DbSet<Campanha> Campanha { get; set; }
        public virtual DbSet<CampanhaArquivo> CampanhaArquivo { get; set; }
        public virtual DbSet<Capa> Capa { get; set; }
        public virtual DbSet<CapaRedeSocial> CapaRedeSocial { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<Cidade> Cidade { get; set; }
        public virtual DbSet<Classificacao> Classificacao { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Corretora> Corretora { get; set; }
        public virtual DbSet<CorretoresCampinas> CorretoresCampinas { get; set; }
        public virtual DbSet<CorretoresSP> CorretoresSP { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<FaixaEtaria> FaixaEtaria { get; set; }
        public virtual DbSet<FaseProposta> FaseProposta { get; set; }
        public virtual DbSet<Fechamento> Fechamento { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<HistoricoAcesso> HistoricoAcesso { get; set; }
        public virtual DbSet<HistoricoLigacao> HistoricoLigacao { get; set; }
        public virtual DbSet<HistoricoProposta> HistoricoProposta { get; set; }
        public virtual DbSet<Informacao> Informacao { get; set; }
        public virtual DbSet<KPIGrupo> KPIGrupo { get; set; }
        public virtual DbSet<KPIGrupoUsuario> KPIGrupoUsuario { get; set; }
        public virtual DbSet<KPIMeta> KPIMeta { get; set; }
        public virtual DbSet<KPIMetaValor> KPIMetaValor { get; set; }
        public virtual DbSet<KPIMetaVida> KPIMetaVida { get; set; }
        public virtual DbSet<Linha> Linha { get; set; }
        public virtual DbSet<Modalidade> Modalidade { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<MotivoDeclinio> MotivoDeclinio { get; set; }
        public virtual DbSet<Notificacao> Notificacao { get; set; }
        public virtual DbSet<NotificacaoMensagem> NotificacaoMensagem { get; set; }
        public virtual DbSet<Operadora> Operadora { get; set; }
        public virtual DbSet<OperadoraDocumento> OperadoraDocumento { get; set; }
        public virtual DbSet<OperadoraTelefone> OperadoraTelefone { get; set; }
        public virtual DbSet<Origem> Origem { get; set; }
        public virtual DbSet<PROPOSTA_BKP> PROPOSTA_BKP { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilModulo> PerfilModulo { get; set; }
        public virtual DbSet<Porte> Porte { get; set; }
        public virtual DbSet<Producao> Producao { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Proposta> Proposta { get; set; }
        public virtual DbSet<PropostaFaixaEtaria> PropostaFaixaEtaria { get; set; }
        public virtual DbSet<RedeSocial> RedeSocial { get; set; }
        public virtual DbSet<StatusProposta> StatusProposta { get; set; }
        public virtual DbSet<StatusVisita> StatusVisita { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoLead> TipoLead { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioCliente> UsuarioCliente { get; set; }
        public virtual DbSet<UsuarioHierarquia> UsuarioHierarquia { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<Visita> Visita { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:app.q2bn.com.br;Initial Catalog=CRMYIA_HOMOLOGACAO;Persist Security Info=False;User ID=user_crmyia;Password=BU7ilv8789twt;MultipleActiveResultSets=False;TrustServerCertificate=False;Connection Timeout=240;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abordagem>(entity =>
            {
                entity.HasKey(e => e.IdAbordagem);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAbordagemCategoriaNavigation)
                    .WithMany(p => p.Abordagem)
                    .HasForeignKey(d => d.IdAbordagemCategoria)
                    .HasConstraintName("AbordagemCategoria_Abordagem");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Abordagem)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Abordagem");
            });

            modelBuilder.Entity<AbordagemCategoria>(entity =>
            {
                entity.HasKey(e => e.IdAbordagemCategoria);

                entity.Property(e => e.IdAbordagemCategoria).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArquivoLead>(entity =>
            {
                entity.HasKey(e => e.IdArquivoLead);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.NomeArquivoOriginal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivoTratado)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AssinaturaCartao>(entity =>
            {
                entity.HasKey(e => e.IdAssinaturaCartao);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.AssinaturaCartao)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_AssinaturaCartao");
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.IdBanner);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdInformacaoNavigation)
                    .WithMany(p => p.Banner)
                    .HasForeignKey(d => d.IdInformacao)
                    .HasConstraintName("Informacao_Banner");
            });

            modelBuilder.Entity<BannerOperadora>(entity =>
            {
                entity.HasKey(e => e.IdBannerOperadora);

                entity.HasOne(d => d.IdBannerNavigation)
                    .WithMany(p => p.BannerOperadora)
                    .HasForeignKey(d => d.IdBanner)
                    .HasConstraintName("Banner_BannerOperadora");

                entity.HasOne(d => d.IdOperadoraNavigation)
                    .WithMany(p => p.BannerOperadora)
                    .HasForeignKey(d => d.IdOperadora)
                    .HasConstraintName("Operadora_BannerOperadora");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.BannerOperadora)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_BannerOperadora");
            });

            modelBuilder.Entity<Campanha>(entity =>
            {
                entity.HasKey(e => e.IdCampanha);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Campanha)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Campanha");
            });

            modelBuilder.Entity<CampanhaArquivo>(entity =>
            {
                entity.HasKey(e => e.IdCampanhaArquivo);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RedesSociais)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPostagem)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCampanhaNavigation)
                    .WithMany(p => p.CampanhaArquivo)
                    .HasForeignKey(d => d.IdCampanha)
                    .HasConstraintName("Campanha_CampanhaArquivo");

                entity.HasOne(d => d.IdInformacaoNavigation)
                    .WithMany(p => p.CampanhaArquivo)
                    .HasForeignKey(d => d.IdInformacao)
                    .HasConstraintName("Informacao_CampanhaArquivo");
            });

            modelBuilder.Entity<Capa>(entity =>
            {
                entity.HasKey(e => e.IdCapa);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CapaRedeSocial>(entity =>
            {
                entity.HasKey(e => e.IdCapaRedeSocial);

                entity.HasOne(d => d.IdCapaNavigation)
                    .WithMany(p => p.CapaRedeSocial)
                    .HasForeignKey(d => d.IdCapa)
                    .HasConstraintName("Capa_CapaRedeSocial");

                entity.HasOne(d => d.IdRedeSocialNavigation)
                    .WithMany(p => p.CapaRedeSocial)
                    .HasForeignKey(d => d.IdRedeSocial)
                    .HasConstraintName("RedeSocial_CapaRedeSocial");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.CapaRedeSocial)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_CapaRedeSocial");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdLinhaNavigation)
                    .WithMany(p => p.Categoria)
                    .HasForeignKey(d => d.IdLinha)
                    .HasConstraintName("Linha_Categoria");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.IdChat);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Mensagem).HasColumnType("text");

                entity.HasOne(d => d.IdUsuarioDeNavigation)
                    .WithMany(p => p.ChatIdUsuarioDeNavigation)
                    .HasForeignKey(d => d.IdUsuarioDe)
                    .HasConstraintName("Usuario_Chat_De");

                entity.HasOne(d => d.IdUsuarioParaNavigation)
                    .WithMany(p => p.ChatIdUsuarioParaNavigation)
                    .HasForeignKey(d => d.IdUsuarioPara)
                    .HasConstraintName("Usuario_Chat_Para");
            });

            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.HasKey(e => e.IdCidade);

                entity.HasIndex(e => new { e.Ativo, e.CodigoIBGE })
                    .HasName("IX_CODIGOIBGE");

                entity.HasIndex(e => new { e.Ativo, e.IdEstado })
                    .HasName("IX_IDESTADO");

                entity.Property(e => e.CodigoIBGE)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Cidade)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("Estado_Cidade");
            });

            modelBuilder.Entity<Classificacao>(entity =>
            {
                entity.HasKey(e => e.IdClassificacao);

                entity.Property(e => e.IdClassificacao).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.HasIndex(e => e.CPF)
                    .HasName("IX_CPF");

                entity.HasIndex(e => new { e.IdCliente, e.CPF })
                    .HasName("IX_IDCLIENTE_CPF");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CEP)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CPF)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CartaoSus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataAdesaoLead).HasColumnType("date");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Endereco)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OperadoraLead)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProdutoLead)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RG)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdArquivoLeadNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdArquivoLead)
                    .HasConstraintName("ArquivoLead_Cliente");

                entity.HasOne(d => d.IdCidadeNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdCidade)
                    .HasConstraintName("Cidade_Cliente");

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .HasConstraintName("EstadoCivil_Cliente");

                entity.HasOne(d => d.IdGeneroNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdGenero)
                    .HasConstraintName("Genero_Cliente");

                entity.HasOne(d => d.IdOrigemNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdOrigem)
                    .HasConstraintName("Origem_Cliente");

                entity.HasOne(d => d.IdTipoLeadNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdTipoLead)
                    .HasConstraintName("TipoLead_Cliente");
            });

            modelBuilder.Entity<Corretora>(entity =>
            {
                entity.HasKey(e => e.IdCorretora);

                entity.Property(e => e.Bairro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CEP)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CNPJ)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Endereco)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeFantasia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCidadeNavigation)
                    .WithMany(p => p.Corretora)
                    .HasForeignKey(d => d.IdCidade)
                    .HasConstraintName("Cidade_Corretora");
            });

            modelBuilder.Entity<CorretoresCampinas>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AcessoLimite)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Acordo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Apelido)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Assistente)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Ativo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Bairro)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BloqueiaComissaoSemNota)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BloqueiaPagtoComissao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BloqueiaProducao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CEP)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CNAE)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CPFResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Cidade)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Coordenador)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorretorClassificacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorretorTipo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Desbloqueios)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DescontaTaxa)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Documento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DtNascimento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EmailResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.HashId)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IncluidoPor)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inclusao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Loja)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MultiNotas)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MultiNotasDescricao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NaoGeraRemessa)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NascimentoResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NomeResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PermiteAntecipacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PermiteDebitoCaixinha)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pessoa)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Producao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Referencia)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Regiao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SUSEP)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Scanner)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SimplesNacional)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UltimaProducao)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CorretoresSP>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.CorretorClassificacao, e.Nome, e.Email, e.Assistente, e.Telefone, e.Producao, e.DtNascimento, e.Documento, e.CNAE, e.SUSEP, e.CPFResponsavel, e.Inclusão, e.CEP, e.Endereco, e.Complemento, e.Bairro, e.Cidade, e.EmailResponsavel, e.UltimaProducao, e.Regiao, e.HashId, e.Codigo })
                    .HasName("IX_CORRETORESP");

                entity.Property(e => e.Assistente)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Bairro)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CEP)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CNAE)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CPFResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Cidade)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CorretorClassificacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Documento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DtNascimento)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EmailResponsavel)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.HashId)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inclusão)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Producao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Regiao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SUSEP)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UltimaProducao)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.HasKey(e => e.IdDocumento);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPropostaNavigation)
                    .WithMany(p => p.Documento)
                    .HasForeignKey(d => d.IdProposta)
                    .HasConstraintName("Proposta_Documento");

                entity.HasOne(d => d.IdTipoDocumentoNavigation)
                    .WithMany(p => p.Documento)
                    .HasForeignKey(d => d.IdTipoDocumento)
                    .HasConstraintName("TipoDocumento_Documento");
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.HasKey(e => e.IdEmail);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.EmailConta)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Email)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("Cliente_Email");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.Property(e => e.IdEstado).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCivil);

                entity.Property(e => e.IdEstadoCivil).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FaixaEtaria>(entity =>
            {
                entity.HasKey(e => e.IdFaixaEtaria);

                entity.Property(e => e.IdFaixaEtaria).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FaseProposta>(entity =>
            {
                entity.HasKey(e => e.IdFaseProposta);

                entity.Property(e => e.IdFaseProposta).ValueGeneratedOnAdd();

                entity.Property(e => e.CorPrincipal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CorSecundaria)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoDetalhada)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Fechamento>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Apelido)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Assistente_Original)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Beneficiario_Producao)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Corretor)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Grade_utilizada)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Modalidade)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Operadora)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Producao_Classificacao)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Segurado)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero);

                entity.Property(e => e.IdGenero).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HistoricoAcesso>(entity =>
            {
                entity.HasKey(e => e.IdHistoricoAcesso);

                entity.Property(e => e.DataAcesso).HasColumnType("datetime");

                entity.Property(e => e.IP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.HistoricoAcesso)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_HistoricoAcesso");
            });

            modelBuilder.Entity<HistoricoLigacao>(entity =>
            {
                entity.HasKey(e => e.IdHistoricoLigacao);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPropostaNavigation)
                    .WithMany(p => p.HistoricoLigacao)
                    .HasForeignKey(d => d.IdProposta)
                    .HasConstraintName("Proposta_HistoricoLigacao");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.HistoricoLigacao)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_HistoricoLigacao");
            });

            modelBuilder.Entity<HistoricoProposta>(entity =>
            {
                entity.HasKey(e => e.IdHistoricoProposta);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPropostaNavigation)
                    .WithMany(p => p.HistoricoProposta)
                    .HasForeignKey(d => d.IdProposta)
                    .HasConstraintName("Proposta_HistoricoProposta");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.HistoricoProposta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_HistoricoProposta");
            });

            modelBuilder.Entity<Informacao>(entity =>
            {
                entity.HasKey(e => e.IdInformacao);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KPIGrupo>(entity =>
            {
                entity.HasKey(e => e.IdKPIGrupo);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KPIGrupoUsuario>(entity =>
            {
                entity.HasKey(e => e.IdKPIGrupoUsuario);

                entity.Property(e => e.CaminhoFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inicio).HasColumnType("datetime");

                entity.Property(e => e.Motivo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Saida).HasColumnType("datetime");

                entity.HasOne(d => d.IdKPIGrupoNavigation)
                    .WithMany(p => p.KPIGrupoUsuario)
                    .HasForeignKey(d => d.IdKPIGrupo)
                    .HasConstraintName("KPIGrupo_KPIGrupoUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.KPIGrupoUsuario)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_KPIGrupoUsuario");
            });

            modelBuilder.Entity<KPIMeta>(entity =>
            {
                entity.HasKey(e => e.IdMeta);

                entity.Property(e => e.DataMaxima).HasColumnType("datetime");

                entity.Property(e => e.DataMinima).HasColumnType("datetime");

                entity.HasOne(d => d.IdKPIGrupoUsuarioNavigation)
                    .WithMany(p => p.KPIMeta)
                    .HasForeignKey(d => d.IdKPIGrupoUsuario)
                    .HasConstraintName("KPIGrupoUsuario_KPIMeta");
            });

            modelBuilder.Entity<KPIMetaValor>(entity =>
            {
                entity.HasKey(e => e.IdKPIMetaValor);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMaximo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorMinimo).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdMetaNavigation)
                    .WithMany(p => p.KPIMetaValor)
                    .HasForeignKey(d => d.IdMeta)
                    .HasConstraintName("KPIMeta_KPIMetaValor");
            });

            modelBuilder.Entity<KPIMetaVida>(entity =>
            {
                entity.HasKey(e => e.IdKPIMetaVida);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMaximo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorMinimo).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdMetaNavigation)
                    .WithMany(p => p.KPIMetaVida)
                    .HasForeignKey(d => d.IdMeta)
                    .HasConstraintName("KPIMeta_KPIMetaVida");
            });

            modelBuilder.Entity<Linha>(entity =>
            {
                entity.HasKey(e => e.IdLinha);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdProdutoNavigation)
                    .WithMany(p => p.Linha)
                    .HasForeignKey(d => d.IdProduto)
                    .HasConstraintName("Produto_Linha");
            });

            modelBuilder.Entity<Modalidade>(entity =>
            {
                entity.HasKey(e => e.IdModalidade);

                entity.Property(e => e.IdModalidade).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo);

                entity.Property(e => e.CssClass)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ToolTip)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModuloReferenciaNavigation)
                    .WithMany(p => p.InverseIdModuloReferenciaNavigation)
                    .HasForeignKey(d => d.IdModuloReferencia)
                    .HasConstraintName("Modulo_Modulo");
            });

            modelBuilder.Entity<MotivoDeclinio>(entity =>
            {
                entity.HasKey(e => e.IdMotivoDeclinio);

                entity.Property(e => e.IdMotivoDeclinio).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Notificacao>(entity =>
            {
                entity.HasKey(e => e.IdNotificacao);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioCadastroNavigation)
                    .WithMany(p => p.NotificacaoIdUsuarioCadastroNavigation)
                    .HasForeignKey(d => d.IdUsuarioCadastro)
                    .HasConstraintName("Usuario_Notificacao_Cadastro");

                entity.HasOne(d => d.IdUsuarioVisualizarNavigation)
                    .WithMany(p => p.NotificacaoIdUsuarioVisualizarNavigation)
                    .HasForeignKey(d => d.IdUsuarioVisualizar)
                    .HasConstraintName("Usuario_Notificacao_Visualizar");
            });

            modelBuilder.Entity<NotificacaoMensagem>(entity =>
            {
                entity.HasKey(e => e.IdNotificacaoMensagem)
                    .HasName("PK__Notifica__60F1092ADE335413");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Mensagem)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioDeNavigation)
                    .WithMany(p => p.NotificacaoMensagemIdUsuarioDeNavigation)
                    .HasForeignKey(d => d.IdUsuarioDe)
                    .HasConstraintName("Usuario_NotificacaoMensagem_De");

                entity.HasOne(d => d.IdUsuarioParaNavigation)
                    .WithMany(p => p.NotificacaoMensagemIdUsuarioParaNavigation)
                    .HasForeignKey(d => d.IdUsuarioPara)
                    .HasConstraintName("Usuario_NotificacaoMensagem_Para");
            });

            modelBuilder.Entity<Operadora>(entity =>
            {
                entity.HasKey(e => e.IdOperadora);

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OperadoraDocumento>(entity =>
            {
                entity.HasKey(e => e.IdOperadoraDocumento);

                entity.Property(e => e.IdOperadoraDocumento).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdOperadoraNavigation)
                    .WithMany(p => p.OperadoraDocumento)
                    .HasForeignKey(d => d.IdOperadora)
                    .HasConstraintName("Operadora_OperadoraDocumento");

                entity.HasOne(d => d.IdTipoDocumentoNavigation)
                    .WithMany(p => p.OperadoraDocumento)
                    .HasForeignKey(d => d.IdTipoDocumento)
                    .HasConstraintName("TipoDocumento_OperadoraDocumento");
            });

            modelBuilder.Entity<OperadoraTelefone>(entity =>
            {
                entity.HasKey(e => e.IdOperadoraTelefone);

                entity.Property(e => e.IdOperadoraTelefone).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Origem>(entity =>
            {
                entity.HasKey(e => e.IdOrigem);

                entity.Property(e => e.IdOrigem).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PROPOSTA_BKP>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataSolicitacao).HasColumnType("datetime");

                entity.Property(e => e.IdProposta).ValueGeneratedOnAdd();

                entity.Property(e => e.NumeroProposta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodoParaLigar)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoJaUtilizado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PreferenciaHospitalar)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ProximoContatoComCliente).HasColumnType("datetime");

                entity.Property(e => e.TempoPlano)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorPrevisto).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.Property(e => e.IdPerfil).ValueGeneratedOnAdd();

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PerfilModulo>(entity =>
            {
                entity.HasKey(e => e.IdPerfilModulo);

                entity.HasIndex(e => new { e.Ativo, e.IdPerfil, e.IdModulo })
                    .HasName("IX_PERFILMODULO");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.PerfilModulo)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("Modulo_PerfilModulo");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.PerfilModulo)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("Perfil_PerfilModulo");
            });

            modelBuilder.Entity<Porte>(entity =>
            {
                entity.HasKey(e => e.IdPorte);

                entity.Property(e => e.IdPorte).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producao>(entity =>
            {
                entity.HasKey(e => e.IdProducao);

                entity.Property(e => e.IdProducao).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.IdProduto);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoDetalhada)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RegistroANS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistroPlano)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOperadoraNavigation)
                    .WithMany(p => p.Produto)
                    .HasForeignKey(d => d.IdOperadora)
                    .HasConstraintName("Operadora_Produto");
            });

            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.HasKey(e => e.IdProposta);

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("IX_IDUSUARIO");

                entity.HasIndex(e => new { e.Ativo, e.IdUsuario })
                    .HasName("IX_PROPOSTA");

                entity.HasIndex(e => new { e.Ativo, e.IdUsuarioCorretor, e.DataSolicitacao })
                    .HasName("IX_ATIVO_IDUSUARIOCORRETOR_DATASOLICITACAO");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataSolicitacao).HasColumnType("datetime");

                entity.Property(e => e.NumeroProposta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodoParaLigar)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoJaUtilizado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PreferenciaHospitalar)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ProximoContatoComCliente).HasColumnType("datetime");

                entity.Property(e => e.TempoPlano)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorPrevisto).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("Categoria_Proposta");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("Cliente_Proposta");

                entity.HasOne(d => d.IdFasePropostaNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdFaseProposta)
                    .HasConstraintName("FaseProposta_Proposta");

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdModalidade)
                    .HasConstraintName("Modalidade_Proposta");

                entity.HasOne(d => d.IdMotivoDeclinioNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdMotivoDeclinio)
                    .HasConstraintName("MotivoDeclinio_Proposta");

                entity.HasOne(d => d.IdPorteNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdPorte)
                    .HasConstraintName("Porte_Proposta");

                entity.HasOne(d => d.IdStatusPropostaNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdStatusProposta)
                    .HasConstraintName("StatusProposta_Proposta");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PropostaIdUsuarioNavigation)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Proposta");

                entity.HasOne(d => d.IdUsuarioCorretorNavigation)
                    .WithMany(p => p.PropostaIdUsuarioCorretorNavigation)
                    .HasForeignKey(d => d.IdUsuarioCorretor)
                    .HasConstraintName("UsuarioCorretor_Proposta");
            });

            modelBuilder.Entity<PropostaFaixaEtaria>(entity =>
            {
                entity.HasKey(e => e.IdPropostaFaixaEtaria);

                entity.HasOne(d => d.IdFaixaEtariaNavigation)
                    .WithMany(p => p.PropostaFaixaEtaria)
                    .HasForeignKey(d => d.IdFaixaEtaria)
                    .HasConstraintName("FaixaEtaria_PropostaFaixaEtaria");

                entity.HasOne(d => d.IdPropostaNavigation)
                    .WithMany(p => p.PropostaFaixaEtaria)
                    .HasForeignKey(d => d.IdProposta)
                    .HasConstraintName("Proposta_PropostaFaixaEtaria");
            });

            modelBuilder.Entity<RedeSocial>(entity =>
            {
                entity.HasKey(e => e.IdRedeSocial);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusProposta>(entity =>
            {
                entity.HasKey(e => e.IdStatusProposta);

                entity.Property(e => e.IdStatusProposta).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusVisita>(entity =>
            {
                entity.HasKey(e => e.IdStatusVisita);

                entity.Property(e => e.IdStatusVisita).ValueGeneratedOnAdd();

                entity.Property(e => e.CorHexa)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CssClass)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.HasKey(e => e.IdTelefone);

                entity.Property(e => e.DDD)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Telefone1)
                    .HasColumnName("Telefone")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Telefone)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("Cliente_Telefone");

                entity.HasOne(d => d.IdOperadoraTelefoneNavigation)
                    .WithMany(p => p.Telefone)
                    .HasForeignKey(d => d.IdOperadoraTelefone)
                    .HasConstraintName("OperadoraTelefone_Telefone");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.HasKey(e => e.IdTipoDocumento);

                entity.Property(e => e.IdTipoDocumento).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoLead>(entity =>
            {
                entity.HasKey(e => e.IdTipoLead);

                entity.Property(e => e.IdTipoLead).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.HasIndex(e => e.Ativo)
                    .HasName("IX_CORRETORESP");

                entity.HasIndex(e => new { e.IdUsuario, e.Ativo })
                    .HasName("IX_IDUSUARIO_ATIVO");

                entity.HasIndex(e => new { e.IdUsuario, e.Logado })
                    .HasName("IX_LOGADO");

                entity.HasIndex(e => new { e.Login, e.Senha })
                    .HasName("IX_LOGIN_SENHA_SEM_ATIVO");

                entity.HasIndex(e => new { e.Ativo, e.Login, e.Senha })
                    .HasName("IX_LOGIN_SENHA");

                entity.Property(e => e.CaminhoFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimentoAbertura).HasColumnType("datetime");

                entity.Property(e => e.Documento)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Logado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeApelido)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClassificacaoNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdClassificacao)
                    .HasConstraintName("Classificacao_Usuario");

                entity.HasOne(d => d.IdCorretoraNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdCorretora)
                    .HasConstraintName("Corretora_Usuario");

                entity.HasOne(d => d.IdProducaoNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdProducao)
                    .HasConstraintName("Producao_Usuario");
            });

            modelBuilder.Entity<UsuarioCliente>(entity =>
            {
                entity.HasKey(e => e.IdUsuarioCliente);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.UsuarioCliente)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("Cliente_UsuarioCliente");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuarioCliente)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_UsuarioCliente");
            });

            modelBuilder.Entity<UsuarioHierarquia>(entity =>
            {
                entity.HasKey(e => e.IdUsuarioHierarquia);

                entity.HasIndex(e => new { e.Ativo, e.IdUsuarioMaster })
                    .HasName("IX_IDUSUARIOMASTER");

                entity.HasIndex(e => new { e.Ativo, e.IdUsuarioSlave })
                    .HasName("IX_IDUSUARIOSLAVE");

                entity.HasIndex(e => new { e.Ativo, e.IdUsuarioMaster, e.IdUsuarioSlave })
                    .HasName("IX_IDUSUARIOMASTER_IDUSUARIOSLAVE");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.IdUsuarioMasterNavigation)
                    .WithMany(p => p.UsuarioHierarquiaIdUsuarioMasterNavigation)
                    .HasForeignKey(d => d.IdUsuarioMaster)
                    .HasConstraintName("Usuario_UsuarioHierarquiaMasterMaster");

                entity.HasOne(d => d.IdUsuarioSlaveNavigation)
                    .WithMany(p => p.UsuarioHierarquiaIdUsuarioSlaveNavigation)
                    .HasForeignKey(d => d.IdUsuarioSlave)
                    .HasConstraintName("Usuario_UsuarioHierarquiaSlaveSlave");
            });

            modelBuilder.Entity<UsuarioPerfil>(entity =>
            {
                entity.HasKey(e => e.IdUsuarioPerfil);

                entity.HasIndex(e => new { e.Ativo, e.IdUsuario, e.IdPerfil })
                    .HasName("IX_IDUSUARIO_IDPERFIL");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("Perfil_UsuarioPerfil");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_UsuarioPerfil");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(e => e.IdVideo)
                    .HasName("PK__Video__54BA87FA4966F1CF");

                entity.Property(e => e.CaminhoArquivo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.IdentificadorVideo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeVideo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Video)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Video");
            });

            modelBuilder.Entity<Visita>(entity =>
            {
                entity.HasKey(e => e.IdVisita);

                entity.Property(e => e.DataAgendamento).HasColumnType("datetime");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataVisitaRealizada).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPropostaNavigation)
                    .WithMany(p => p.Visita)
                    .HasForeignKey(d => d.IdProposta)
                    .HasConstraintName("Proposta_Visita");

                entity.HasOne(d => d.IdStatusVisitaNavigation)
                    .WithMany(p => p.Visita)
                    .HasForeignKey(d => d.IdStatusVisita)
                    .HasConstraintName("StatusVisita_Visita");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Visita)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Visita");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
