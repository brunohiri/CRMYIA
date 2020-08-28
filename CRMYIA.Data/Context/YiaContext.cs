﻿using System;
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

        public virtual DbSet<Cidade> Cidade { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Corretora> Corretora { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<FaseProposta> FaseProposta { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<HistoricoAcesso> HistoricoAcesso { get; set; }
        public virtual DbSet<HistoricoProposta> HistoricoProposta { get; set; }
        public virtual DbSet<Meta> Meta { get; set; }
        public virtual DbSet<Modalidade> Modalidade { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<MotivoDeclinio> MotivoDeclinio { get; set; }
        public virtual DbSet<Operadora> Operadora { get; set; }
        public virtual DbSet<OperadoraDocumento> OperadoraDocumento { get; set; }
        public virtual DbSet<OperadoraTelefone> OperadoraTelefone { get; set; }
        public virtual DbSet<Origem> Origem { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilModulo> PerfilModulo { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Proposta> Proposta { get; set; }
        public virtual DbSet<StatusProposta> StatusProposta { get; set; }
        public virtual DbSet<StatusVisita> StatusVisita { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoLead> TipoLead { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioCliente> UsuarioCliente { get; set; }
        public virtual DbSet<UsuarioHierarquia> UsuarioHierarquia { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<Visita> Visita { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:emissor.database.windows.net,1433;Initial Catalog=crmyia;Persist Security Info=False;User ID=userApp;Password=(#Emissor#2020$);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.HasKey(e => e.IdCidade);

                entity.Property(e => e.IdCidade).ValueGeneratedOnAdd();

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

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

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

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RG)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCidadeNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdCidade)
                    .HasConstraintName("Cidade_Cliente");

                entity.HasOne(d => d.IdEmailNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdEmail)
                    .HasConstraintName("Email_Cliente");

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

                entity.HasOne(d => d.IdTelefoneNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdTelefone)
                    .HasConstraintName("Telefone_Cliente");

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

            modelBuilder.Entity<FaseProposta>(entity =>
            {
                entity.HasKey(e => e.IdFaseProposta);

                entity.Property(e => e.IdFaseProposta).ValueGeneratedOnAdd();

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

            modelBuilder.Entity<Meta>(entity =>
            {
                entity.HasKey(e => e.IdMeta);

                entity.Property(e => e.DataMaxima).HasColumnType("datetime");

                entity.Property(e => e.DataMinima).HasColumnType("datetime");

                entity.Property(e => e.ValorMaximo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorMinimo).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Meta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Meta");
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

                entity.Property(e => e.IdModulo).ValueGeneratedNever();

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

            modelBuilder.Entity<Operadora>(entity =>
            {
                entity.HasKey(e => e.IdOperadora);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
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

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.Property(e => e.IdPerfil).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PerfilModulo>(entity =>
            {
                entity.HasKey(e => e.IdPerfilModulo);

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.PerfilModulo)
                    .HasForeignKey(d => d.IdModulo)
                    .HasConstraintName("Modulo_PerfilModulo");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.PerfilModulo)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("Perfil_PerfilModulo");
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

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataSolicitacao).HasColumnType("datetime");

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.PreferenciaHospitalar)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ProximoContatoComCliente).HasColumnType("datetime");

                entity.Property(e => e.TempoPlano)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorPrevisto).HasColumnType("decimal(18, 2)");

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

                entity.HasOne(d => d.IdProdutoNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdProduto)
                    .HasConstraintName("Produto_Proposta");

                entity.HasOne(d => d.IdStatusPropostaNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdStatusProposta)
                    .HasConstraintName("StatusProposta_Proposta");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Proposta");
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

                entity.Property(e => e.CPF)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCorretoraNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdCorretora)
                    .HasConstraintName("Corretora_Usuario");
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

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("Perfil_UsuarioPerfil");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_UsuarioPerfil");
            });

            modelBuilder.Entity<Visita>(entity =>
            {
                entity.HasKey(e => e.IdVisita);

                entity.Property(e => e.DataAgendamento).HasColumnType("datetime");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataVisitaRealizada).HasColumnType("datetime");

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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}