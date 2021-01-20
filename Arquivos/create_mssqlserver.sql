/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.1.0                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          CRM_Model.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2021-01-20 13:13                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Tables                                                                 */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "Usuario"                                                    */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Usuario] (
    [IdUsuario] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCorretora] BIGINT,
    [IdClassificacao] TINYINT,
    [IdProducao] TINYINT,
    [Nome] VARCHAR(200),
    [Documento] VARCHAR(30),
    [DataNascimentoAbertura] DATETIME,
    [Codigo] VARCHAR(50),
    [Telefone] VARCHAR(20),
    [Email] VARCHAR(200),
    [Login] VARCHAR(200),
    [Senha] VARCHAR(200),
    [IP] VARCHAR(20),
    [CaminhoFoto] VARCHAR(500),
    [NomeFoto] VARCHAR(500),
    [Logado] VARCHAR(100),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([IdUsuario])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "HistoricoAcesso"                                            */
/* ---------------------------------------------------------------------- */
CREATE TABLE [HistoricoAcesso] (
    [IdHistoricoAcesso] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [DataAcesso] DATETIME NOT NULL,
    [IP] VARCHAR(20),
    CONSTRAINT [PK_HistoricoAcesso] PRIMARY KEY ([IdHistoricoAcesso])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Perfil"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Perfil] (
    [IdPerfil] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Perfil] PRIMARY KEY ([IdPerfil])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "UsuarioHierarquia"                                          */
/* ---------------------------------------------------------------------- */
CREATE TABLE [UsuarioHierarquia] (
    [IdUsuarioHierarquia] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuarioMaster] BIGINT,
    [IdUsuarioSlave] BIGINT,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_UsuarioHierarquia] PRIMARY KEY ([IdUsuarioHierarquia])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Produto"                                                    */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Produto] (
    [IdProduto] BIGINT IDENTITY(1,1) NOT NULL,
    [IdOperadora] BIGINT,
    [Descricao] VARCHAR(200),
    [DescricaoDetalhada] VARCHAR(500),
    [RegistroANS] VARCHAR(50),
    [RegistroPlano] VARCHAR(50),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Produto] PRIMARY KEY ([IdProduto])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Operadora"                                                  */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Operadora] (
    [IdOperadora] BIGINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Operadora] PRIMARY KEY ([IdOperadora])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Cliente"                                                    */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Cliente] (
    [IdCliente] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCidade] INTEGER,
    [IdEstadoCivil] TINYINT,
    [IdGenero] TINYINT,
    [IdOrigem] TINYINT,
    [IdTipoLead] TINYINT,
    [IdArquivoLead] BIGINT,
    [Nome] VARCHAR(200),
    [CPF] VARCHAR(20),
    [RG] VARCHAR(20),
    [CartaoSus] VARCHAR(50),
    [DataNascimento] DATETIME,
    [Idade] INTEGER,
    [CEP] VARCHAR(10),
    [Endereco] VARCHAR(200),
    [Numero] VARCHAR(20),
    [Bairro] VARCHAR(200),
    [Complemento] VARCHAR(200),
    [Observacao] VARCHAR(500),
    [OperadoraLead] VARCHAR(200),
    [PlanoLead] VARCHAR(200),
    [StatusPlanoLead] BIT,
    [DataAdesaoLead] DATE,
    [IsLead] BIT NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Cliente] PRIMARY KEY ([IdCliente])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Estado"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Estado] (
    [IdEstado] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Sigla] VARCHAR(2),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Estado] PRIMARY KEY ([IdEstado])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Cidade"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Cidade] (
    [IdCidade] INTEGER IDENTITY(1,1) NOT NULL,
    [IdEstado] TINYINT,
    [Descricao] VARCHAR(200),
    [CodigoIBGE] VARCHAR(20),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Cidade] PRIMARY KEY ([IdCidade])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Telefone"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Telefone] (
    [IdTelefone] BIGINT IDENTITY(1,1) NOT NULL,
    [IdOperadoraTelefone] TINYINT,
    [IdCliente] BIGINT,
    [DDD] VARCHAR(2),
    [Telefone] VARCHAR(10),
    [WhatsApp] BIT NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Telefone] PRIMARY KEY ([IdTelefone])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "OperadoraTelefone"                                          */
/* ---------------------------------------------------------------------- */
CREATE TABLE [OperadoraTelefone] (
    [IdOperadoraTelefone] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_OperadoraTelefone] PRIMARY KEY ([IdOperadoraTelefone])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Email"                                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Email] (
    [IdEmail] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCliente] BIGINT,
    [EmailConta] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Email] PRIMARY KEY ([IdEmail])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "EstadoCivil"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [EstadoCivil] (
    [IdEstadoCivil] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_EstadoCivil] PRIMARY KEY ([IdEstadoCivil])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Genero"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Genero] (
    [IdGenero] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Genero] PRIMARY KEY ([IdGenero])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Origem"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Origem] (
    [IdOrigem] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Origem] PRIMARY KEY ([IdOrigem])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "UsuarioCliente"                                             */
/* ---------------------------------------------------------------------- */
CREATE TABLE [UsuarioCliente] (
    [IdUsuarioCliente] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [IdCliente] BIGINT,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_UsuarioCliente] PRIMARY KEY ([IdUsuarioCliente])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Proposta"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Proposta] (
    [IdProposta] BIGINT IDENTITY(1,1) NOT NULL,
    [IdModalidade] TINYINT,
    [IdCategoria] BIGINT,
    [IdPorte] TINYINT,
    [IdCliente] BIGINT,
    [IdUsuario] BIGINT,
    [IdUsuarioCorretor] BIGINT,
    [IdStatusProposta] TINYINT,
    [IdMotivoDeclinio] TINYINT,
    [IdFaseProposta] TINYINT,
    [DataSolicitacao] DATETIME,
    [ProximoContatoComCliente] DATETIME,
    [HorarioParaLigar] TIME,
    [PeriodoParaLigar] VARCHAR(50),
    [ValorPrevisto] DECIMAL(18,2),
    [QuantidadeVidas] INTEGER,
    [PossuiPlano] BIT NOT NULL,
    [PlanoJaUtilizado] VARCHAR(200),
    [TempoPlano] VARCHAR(200),
    [PreferenciaHospitalar] VARCHAR(500),
    [Observacoes] VARCHAR(2000),
    [NumeroProposta] VARCHAR(50),
    [Probabilidade] INTEGER,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Proposta] PRIMARY KEY ([IdProposta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Modalidade"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Modalidade] (
    [IdModalidade] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Modalidade] PRIMARY KEY ([IdModalidade])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusProposta"                                             */
/* ---------------------------------------------------------------------- */
CREATE TABLE [StatusProposta] (
    [IdStatusProposta] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_StatusProposta] PRIMARY KEY ([IdStatusProposta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "HistoricoProposta"                                          */
/* ---------------------------------------------------------------------- */
CREATE TABLE [HistoricoProposta] (
    [IdHistoricoProposta] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProposta] BIGINT,
    [IdUsuario] BIGINT,
    [Observacao] VARCHAR(1000),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_HistoricoProposta] PRIMARY KEY ([IdHistoricoProposta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "MotivoDeclinio"                                             */
/* ---------------------------------------------------------------------- */
CREATE TABLE [MotivoDeclinio] (
    [IdMotivoDeclinio] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_MotivoDeclinio] PRIMARY KEY ([IdMotivoDeclinio])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "TipoLead"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [TipoLead] (
    [IdTipoLead] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    [IdTipoLead1] TINYINT,
    CONSTRAINT [PK_TipoLead] PRIMARY KEY ([IdTipoLead])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "FaseProposta"                                               */
/* ---------------------------------------------------------------------- */
CREATE TABLE [FaseProposta] (
    [IdFaseProposta] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [DescricaoDetalhada] VARCHAR(1000),
    [TempoLimite] INTEGER,
    [Observacao] VARCHAR(1000),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_FaseProposta] PRIMARY KEY ([IdFaseProposta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Documento"                                                  */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Documento] (
    [IdDocumento] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProposta] BIGINT,
    [IdTipoDocumento] TINYINT,
    [Descricao] VARCHAR(200),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Observacao] VARCHAR(1000),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Documento] PRIMARY KEY ([IdDocumento])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "TipoDocumento"                                              */
/* ---------------------------------------------------------------------- */
CREATE TABLE [TipoDocumento] (
    [IdTipoDocumento] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_TipoDocumento] PRIMARY KEY ([IdTipoDocumento])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "OperadoraDocumento"                                         */
/* ---------------------------------------------------------------------- */
CREATE TABLE [OperadoraDocumento] (
    [IdOperadoraDocumento] TINYINT IDENTITY(1,1) NOT NULL,
    [IdOperadora] BIGINT,
    [IdTipoDocumento] TINYINT,
    [Obrigatorio] BIT NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_OperadoraDocumento] PRIMARY KEY ([IdOperadoraDocumento])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Meta"                                                       */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Meta] (
    [IdMeta] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [ValorMinimo] DECIMAL(18,2),
    [ValorMaximo] DECIMAL(18,2),
    [DataMinima] DATETIME NOT NULL,
    [DataMaxima] DATETIME,
    [Ativo] BIT NOT NULL,
    [IdKPIMetaVida] BIGINT,
    [IdKPIServico] BIGINT,
    CONSTRAINT [PK_Meta] PRIMARY KEY ([IdMeta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Visita"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Visita] (
    [IdVisita] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProposta] BIGINT,
    [IdStatusVisita] TINYINT,
    [IdUsuario] BIGINT,
    [Descricao] VARCHAR(200),
    [DataAgendamento] DATETIME,
    [DataCadastro] DATETIME NOT NULL,
    [DataVisitaRealizada] DATETIME,
    [Observacao] VARCHAR(500),
    CONSTRAINT [PK_Visita] PRIMARY KEY ([IdVisita])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "StatusVisita"                                               */
/* ---------------------------------------------------------------------- */
CREATE TABLE [StatusVisita] (
    [IdStatusVisita] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [CorHexa] VARCHAR(20),
    [CssClass] VARCHAR(20),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_StatusVisita] PRIMARY KEY ([IdStatusVisita])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Corretora"                                                  */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Corretora] (
    [IdCorretora] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCidade] INTEGER,
    [CNPJ] VARCHAR(20),
    [RazaoSocial] VARCHAR(200),
    [NomeFantasia] VARCHAR(200),
    [CEP] VARCHAR(10),
    [Endereco] VARCHAR(200),
    [Numero] VARCHAR(20),
    [Bairro] VARCHAR(200),
    [Complemento] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Corretora] PRIMARY KEY ([IdCorretora])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Modulo"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Modulo] (
    [IdModulo] BIGINT IDENTITY(1,1) NOT NULL,
    [IdModuloReferencia] BIGINT,
    [Descricao] VARCHAR(200),
    [Url] VARCHAR(200),
    [CssClass] VARCHAR(200),
    [ToolTip] VARCHAR(200),
    [Ordem] TINYINT,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Modulo] PRIMARY KEY ([IdModulo])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "PerfilModulo"                                               */
/* ---------------------------------------------------------------------- */
CREATE TABLE [PerfilModulo] (
    [IdPerfilModulo] BIGINT IDENTITY(1,1) NOT NULL,
    [IdPerfil] TINYINT,
    [IdModulo] BIGINT,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_PerfilModulo] PRIMARY KEY ([IdPerfilModulo])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "UsuarioPerfil"                                              */
/* ---------------------------------------------------------------------- */
CREATE TABLE [UsuarioPerfil] (
    [IdUsuarioPerfil] BIGINT IDENTITY(1,1) NOT NULL,
    [IdPerfil] TINYINT,
    [IdUsuario] BIGINT,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_UsuarioPerfil] PRIMARY KEY ([IdUsuarioPerfil])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "FaixaEtaria"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [FaixaEtaria] (
    [IdFaixaEtaria] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_FaixaEtaria] PRIMARY KEY ([IdFaixaEtaria])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "PropostaFaixaEtaria"                                        */
/* ---------------------------------------------------------------------- */
CREATE TABLE [PropostaFaixaEtaria] (
    [IdPropostaFaixaEtaria] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProposta] BIGINT,
    [IdFaixaEtaria] TINYINT,
    [Quantidade] INTEGER,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_PropostaFaixaEtaria] PRIMARY KEY ([IdPropostaFaixaEtaria])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Classificacao"                                              */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Classificacao] (
    [IdClassificacao] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Classificacao] PRIMARY KEY ([IdClassificacao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Producao"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Producao] (
    [IdProducao] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Producao] PRIMARY KEY ([IdProducao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "KPIMetaValor"                                               */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIMetaValor] (
    [IdKPIMetaValor] BIGINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Meta] DECIMAL(18,2),
    [Mes] TINYINT,
    [Ano] INTEGER,
    [Ativo] BIT NOT NULL,
    [Estipulado] DECIMAL(18,2),
    [IdMeta] BIGINT,
    CONSTRAINT [PK_KPIMetaValor] PRIMARY KEY ([IdKPIMetaValor])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Linha"                                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Linha] (
    [IdLinha] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProduto] BIGINT,
    [Descricao] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Linha] PRIMARY KEY ([IdLinha])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Categoria"                                                  */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Categoria] (
    [IdCategoria] BIGINT IDENTITY(1,1) NOT NULL,
    [IdLinha] BIGINT,
    [Descricao] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Categoria] PRIMARY KEY ([IdCategoria])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Porte"                                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Porte] (
    [IdPorte] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Porte] PRIMARY KEY ([IdPorte])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "ArquivoLead"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [ArquivoLead] (
    [IdArquivoLead] BIGINT IDENTITY(1,1) NOT NULL,
    [CaminhoArquivo] VARCHAR(200),
    [NomeArquivoOriginal] VARCHAR(200),
    [NomeArquivoTratado] VARCHAR(200),
    [QtdRegistros] INTEGER,
    [DataCadastro] DATETIME NOT NULL,
    CONSTRAINT [PK_ArquivoLead] PRIMARY KEY ([IdArquivoLead])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Notificacao"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Notificacao] (
    [IdNotificacao] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuarioCadastro] BIGINT,
    [IdUsuarioVisualizar] BIGINT,
    [Titulo] VARCHAR(200),
    [Descricao] VARCHAR(500),
    [Url] VARCHAR(200),
    [Visualizado] BIT NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Notificacao] PRIMARY KEY ([IdNotificacao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "HistoricoLigacao"                                           */
/* ---------------------------------------------------------------------- */
CREATE TABLE [HistoricoLigacao] (
    [IdHistoricoLigacao] BIGINT IDENTITY(0,1) NOT NULL,
    [IdProposta] BIGINT,
    [IdUsuario] BIGINT,
    [Observacao] VARCHAR(500),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_HistoricoLigacao] PRIMARY KEY ([IdHistoricoLigacao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Campanha"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Campanha] (
    [IdCampanha] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [Descricao] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Campanha] PRIMARY KEY ([IdCampanha])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "CampanhaArquivo"                                            */
/* ---------------------------------------------------------------------- */
CREATE TABLE [CampanhaArquivo] (
    [IdCampanhaArquivo] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCampanha] BIGINT,
    [Descricao] VARCHAR(200),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Observacao] VARCHAR(1000),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_CampanhaArquivo] PRIMARY KEY ([IdCampanhaArquivo])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "KPIMetaVida"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIMetaVida] (
    [IdKPIMetaVida] BIGINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Meta] DECIMAL(18,2),
    [Mes] TINYINT,
    [Ano] INTEGER,
    [Ativo] BIT NOT NULL,
    [Estipulado] DECIMAL(18,2),
    [IdMeta] BIGINT,
    CONSTRAINT [PK_KPIMetaVida] PRIMARY KEY ([IdKPIMetaVida])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "KPIServico"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIServico] (
    [IdKPIServico] BIGINT IDENTITY(1,1) NOT NULL,
    [Perfil] VARCHAR(40),
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_KPIServico] PRIMARY KEY ([IdKPIServico])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Chat"                                                       */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Chat] (
    [IdChat] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuarioDe] BIGINT,
    [IdUsuarioPara] BIGINT,
    [Mensagem] TEXT,
    [DataCadastro] DATETIME NOT NULL,
    [Visualizado] BIT NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Chat] PRIMARY KEY ([IdChat])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "NotificacaoMensagem"                                        */
/* ---------------------------------------------------------------------- */
CREATE TABLE [NotificacaoMensagem] (
    [IdNotificacaoMensagem] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuarioDe] BIGINT,
    [IdUsuarioPara] BIGINT,
    [Mensagem] VARCHAR(100),
    [DataCadastro] DATETIME NOT NULL,
    [Visualizado] BIT NOT NULL,
    [Ativo] BIT NOT NULL,
    PRIMARY KEY ([IdNotificacaoMensagem])
)
GO


/* ---------------------------------------------------------------------- */
/* Foreign key constraints                                                */
/* ---------------------------------------------------------------------- */
ALTER TABLE [Usuario] ADD CONSTRAINT [Corretora_Usuario] 
    FOREIGN KEY ([IdCorretora]) REFERENCES [Corretora] ([IdCorretora])
GO

ALTER TABLE [Usuario] ADD CONSTRAINT [Classificacao_Usuario] 
    FOREIGN KEY ([IdClassificacao]) REFERENCES [Classificacao] ([IdClassificacao])
GO

ALTER TABLE [Usuario] ADD CONSTRAINT [Producao_Usuario] 
    FOREIGN KEY ([IdProducao]) REFERENCES [Producao] ([IdProducao])
GO

ALTER TABLE [HistoricoAcesso] ADD CONSTRAINT [Usuario_HistoricoAcesso] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [UsuarioHierarquia] ADD CONSTRAINT [Usuario_UsuarioHierarquiaMasterMaster] 
    FOREIGN KEY ([IdUsuarioMaster]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [UsuarioHierarquia] ADD CONSTRAINT [Usuario_UsuarioHierarquiaSlaveSlave] 
    FOREIGN KEY ([IdUsuarioSlave]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Produto] ADD CONSTRAINT [Operadora_Produto] 
    FOREIGN KEY ([IdOperadora]) REFERENCES [Operadora] ([IdOperadora])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [Cidade_Cliente] 
    FOREIGN KEY ([IdCidade]) REFERENCES [Cidade] ([IdCidade])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [EstadoCivil_Cliente] 
    FOREIGN KEY ([IdEstadoCivil]) REFERENCES [EstadoCivil] ([IdEstadoCivil])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [Genero_Cliente] 
    FOREIGN KEY ([IdGenero]) REFERENCES [Genero] ([IdGenero])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [Origem_Cliente] 
    FOREIGN KEY ([IdOrigem]) REFERENCES [Origem] ([IdOrigem])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [TipoLead_Cliente] 
    FOREIGN KEY ([IdTipoLead]) REFERENCES [TipoLead] ([IdTipoLead])
GO

ALTER TABLE [Cliente] ADD CONSTRAINT [ArquivoLead_Cliente] 
    FOREIGN KEY ([IdArquivoLead]) REFERENCES [ArquivoLead] ([IdArquivoLead])
GO

ALTER TABLE [Cidade] ADD CONSTRAINT [Estado_Cidade] 
    FOREIGN KEY ([IdEstado]) REFERENCES [Estado] ([IdEstado])
GO

ALTER TABLE [Telefone] ADD CONSTRAINT [OperadoraTelefone_Telefone] 
    FOREIGN KEY ([IdOperadoraTelefone]) REFERENCES [OperadoraTelefone] ([IdOperadoraTelefone])
GO

ALTER TABLE [Telefone] ADD CONSTRAINT [Cliente_Telefone] 
    FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente])
GO

ALTER TABLE [Email] ADD CONSTRAINT [Cliente_Email] 
    FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente])
GO

ALTER TABLE [UsuarioCliente] ADD CONSTRAINT [Usuario_UsuarioCliente] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [UsuarioCliente] ADD CONSTRAINT [Cliente_UsuarioCliente] 
    FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [Modalidade_Proposta] 
    FOREIGN KEY ([IdModalidade]) REFERENCES [Modalidade] ([IdModalidade])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [Cliente_Proposta] 
    FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [Usuario_Proposta] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [StatusProposta_Proposta] 
    FOREIGN KEY ([IdStatusProposta]) REFERENCES [StatusProposta] ([IdStatusProposta])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [MotivoDeclinio_Proposta] 
    FOREIGN KEY ([IdMotivoDeclinio]) REFERENCES [MotivoDeclinio] ([IdMotivoDeclinio])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [FaseProposta_Proposta] 
    FOREIGN KEY ([IdFaseProposta]) REFERENCES [FaseProposta] ([IdFaseProposta])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [UsuarioCorretor_Proposta] 
    FOREIGN KEY ([IdUsuarioCorretor]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [Categoria_Proposta] 
    FOREIGN KEY ([IdCategoria]) REFERENCES [Categoria] ([IdCategoria])
GO

ALTER TABLE [Proposta] ADD CONSTRAINT [Porte_Proposta] 
    FOREIGN KEY ([IdPorte]) REFERENCES [Porte] ([IdPorte])
GO

ALTER TABLE [HistoricoProposta] ADD CONSTRAINT [Proposta_HistoricoProposta] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [HistoricoProposta] ADD CONSTRAINT [Usuario_HistoricoProposta] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Documento] ADD CONSTRAINT [Proposta_Documento] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [Documento] ADD CONSTRAINT [TipoDocumento_Documento] 
    FOREIGN KEY ([IdTipoDocumento]) REFERENCES [TipoDocumento] ([IdTipoDocumento])
GO

ALTER TABLE [OperadoraDocumento] ADD CONSTRAINT [Operadora_OperadoraDocumento] 
    FOREIGN KEY ([IdOperadora]) REFERENCES [Operadora] ([IdOperadora])
GO

ALTER TABLE [OperadoraDocumento] ADD CONSTRAINT [TipoDocumento_OperadoraDocumento] 
    FOREIGN KEY ([IdTipoDocumento]) REFERENCES [TipoDocumento] ([IdTipoDocumento])
GO

ALTER TABLE [Meta] ADD CONSTRAINT [Usuario_Meta] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Meta] ADD CONSTRAINT [KPIServico_Meta] 
    FOREIGN KEY ([IdKPIServico]) REFERENCES [KPIServico] ([IdKPIServico])
GO

ALTER TABLE [Visita] ADD CONSTRAINT [Proposta_Visita] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [Visita] ADD CONSTRAINT [StatusVisita_Visita] 
    FOREIGN KEY ([IdStatusVisita]) REFERENCES [StatusVisita] ([IdStatusVisita])
GO

ALTER TABLE [Visita] ADD CONSTRAINT [Usuario_Visita] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Corretora] ADD CONSTRAINT [Cidade_Corretora] 
    FOREIGN KEY ([IdCidade]) REFERENCES [Cidade] ([IdCidade])
GO

ALTER TABLE [Modulo] ADD CONSTRAINT [Modulo_Modulo] 
    FOREIGN KEY ([IdModuloReferencia]) REFERENCES [Modulo] ([IdModulo])
GO

ALTER TABLE [PerfilModulo] ADD CONSTRAINT [Modulo_PerfilModulo] 
    FOREIGN KEY ([IdModulo]) REFERENCES [Modulo] ([IdModulo])
GO

ALTER TABLE [PerfilModulo] ADD CONSTRAINT [Perfil_PerfilModulo] 
    FOREIGN KEY ([IdPerfil]) REFERENCES [Perfil] ([IdPerfil])
GO

ALTER TABLE [UsuarioPerfil] ADD CONSTRAINT [Perfil_UsuarioPerfil] 
    FOREIGN KEY ([IdPerfil]) REFERENCES [Perfil] ([IdPerfil])
GO

ALTER TABLE [UsuarioPerfil] ADD CONSTRAINT [Usuario_UsuarioPerfil] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [PropostaFaixaEtaria] ADD CONSTRAINT [Proposta_PropostaFaixaEtaria] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [PropostaFaixaEtaria] ADD CONSTRAINT [FaixaEtaria_PropostaFaixaEtaria] 
    FOREIGN KEY ([IdFaixaEtaria]) REFERENCES [FaixaEtaria] ([IdFaixaEtaria])
GO

ALTER TABLE [KPIMetaValor] ADD CONSTRAINT [Meta_KPIMetaValor] 
    FOREIGN KEY ([IdMeta]) REFERENCES [Meta] ([IdMeta])
GO

ALTER TABLE [Linha] ADD CONSTRAINT [Produto_Linha] 
    FOREIGN KEY ([IdProduto]) REFERENCES [Produto] ([IdProduto])
GO

ALTER TABLE [Categoria] ADD CONSTRAINT [Linha_Categoria] 
    FOREIGN KEY ([IdLinha]) REFERENCES [Linha] ([IdLinha])
GO

ALTER TABLE [Notificacao] ADD CONSTRAINT [Usuario_Notificacao_Cadastro] 
    FOREIGN KEY ([IdUsuarioCadastro]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Notificacao] ADD CONSTRAINT [Usuario_Notificacao_Visualizar] 
    FOREIGN KEY ([IdUsuarioVisualizar]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [HistoricoLigacao] ADD CONSTRAINT [Proposta_HistoricoLigacao] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [HistoricoLigacao] ADD CONSTRAINT [Usuario_HistoricoLigacao] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Campanha] ADD CONSTRAINT [Usuario_Campanha] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [CampanhaArquivo] ADD CONSTRAINT [Campanha_CampanhaArquivo] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [KPIMetaVida] ADD CONSTRAINT [Meta_KPIMetaVida] 
    FOREIGN KEY ([IdMeta]) REFERENCES [Meta] ([IdMeta])
GO

ALTER TABLE [Chat] ADD CONSTRAINT [Usuario_Chat_De] 
    FOREIGN KEY ([IdUsuarioDe]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Chat] ADD CONSTRAINT [Usuario_Chat_Para] 
    FOREIGN KEY ([IdUsuarioPara]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [NotificacaoMensagem] ADD CONSTRAINT [Usuario_NotificacaoMensagem_De] 
    FOREIGN KEY ([IdUsuarioDe]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [NotificacaoMensagem] ADD CONSTRAINT [Usuario_NotificacaoMensagem_Para] 
    FOREIGN KEY ([IdUsuarioPara]) REFERENCES [Usuario] ([IdUsuario])
GO

