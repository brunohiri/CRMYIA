/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.1.0                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          CRM_Model.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2021-06-01 14:33                                */
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
    [IdGrupoCorretor] TINYINT,
    [Nome] VARCHAR(200),
    [NomeApelido] VARCHAR(200),
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
    [Facebook] VARCHAR(200),
    [Twitter] VARCHAR(200),
    [Instagram] VARCHAR(200),
    [Linkedin] VARCHAR(200),
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
    [IdModalidade] TINYINT,
    [Descricao] VARCHAR(200),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
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
    [IdClienteReferencia] BIGINT,
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
    [Titular] BIT NOT NULL,
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
    [IdBanco] BIGINT,
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
    [Agencia] VARCHAR(50),
    [ContaCorrente] VARCHAR(50),
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
    [UsuarioMasterSlave] BIT,
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
    [CorPrincipal] VARCHAR(50),
    [CorSecundaria] VARCHAR(50),
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
/* Add table "KPIMeta"                                                    */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIMeta] (
    [IdMeta] BIGINT IDENTITY(1,1) NOT NULL,
    [IdKPIGrupo] BIGINT,
    [DataMinima] DATETIME NOT NULL,
    [DataMaxima] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_KPIMeta] PRIMARY KEY ([IdMeta])
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
    [IdCalendarioSazonal] BIGINT,
    [DataInicio] DATETIME,
    [DataFim] DATETIME,
    [Visivel] TINYINT,
    [Cor] VARCHAR(40),
    [Tipo] TINYINT,
    [GuidId] VARCHAR(255),
    [Repete] TINYINT,
    [Frequencia] TINYINT,
    [Repetir] INTEGER,
    [Termina] INTEGER,
    [Semana] VARCHAR(300),
    [MesDataColocacao] INTEGER,
    [MesDiaDaSemana] VARCHAR(300),
    [MesDia] INTEGER,
    [SelectMensalmente] INTEGER,
    [DataTerminaEm] DATETIME,
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
    [IdMeta] BIGINT,
    [Descricao] VARCHAR(200),
    [ValorMinimo] DECIMAL(18,2),
    [ValorMaximo] DECIMAL(18,2),
    [Mes] TINYINT,
    [Ano] INTEGER,
    [Ativo] BIT NOT NULL,
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
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [QuantidadeDownload] BIGINT,
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
    [IdInformacao] BIGINT,
    [IdCalendario] BIGINT,
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Width] INTEGER,
    [Height] INTEGER,
    [RedesSociais] VARCHAR(300),
    [TipoPostagem] VARCHAR(300),
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
    [IdMeta] BIGINT,
    [Descricao] VARCHAR(200),
    [ValorMinimo] DECIMAL(18,2),
    [ValorMaximo] DECIMAL(18,2),
    [Mes] TINYINT,
    [Ano] INTEGER,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_KPIMetaVida] PRIMARY KEY ([IdKPIMetaVida])
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
/* Add table "KPIGrupo"                                                   */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIGrupo] (
    [IdKPIGrupo] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [Nome] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_KPIGrupo] PRIMARY KEY ([IdKPIGrupo])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "KPIGrupoUsuario"                                            */
/* ---------------------------------------------------------------------- */
CREATE TABLE [KPIGrupoUsuario] (
    [IdKPIGrupoUsuario] BIGINT IDENTITY(1,1) NOT NULL,
    [IdKPIGrupo] BIGINT,
    [IdUsuario] BIGINT,
    [IdMeta] BIGINT,
    [Nome] VARCHAR(200),
    [Perfil] VARCHAR(200),
    [Inicio] DATETIME NOT NULL,
    [Saida] DATETIME,
    [Motivo] VARCHAR(200),
    [CaminhoFoto] VARCHAR(500),
    [NomeFoto] VARCHAR(500),
    [Grupo] BIT,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_KPIGrupoUsuario] PRIMARY KEY ([IdKPIGrupoUsuario])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Abordagem"                                                  */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Abordagem] (
    [IdAbordagem] BIGINT IDENTITY(1,1) NOT NULL,
    [IdAbordagemCategoria] TINYINT,
    [IdUsuario] BIGINT,
    [Descricao] VARCHAR(5000),
    [Pergunta] BIT NOT NULL,
    [Ordem] TINYINT,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Abordagem] PRIMARY KEY ([IdAbordagem])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AbordagemCategoria"                                         */
/* ---------------------------------------------------------------------- */
CREATE TABLE [AbordagemCategoria] (
    [IdAbordagemCategoria] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_AbordagemCategoria] PRIMARY KEY ([IdAbordagemCategoria])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Video"                                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Video] (
    [IdVideo] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [IdCampanha] BIGINT,
    [IdCalendario] BIGINT,
    [IdentificadorVideo] VARCHAR(200),
    [CaminhoArquivo] VARCHAR(500),
    [NomeVideo] TEXT,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    PRIMARY KEY ([IdVideo])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "RedeSocial"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [RedeSocial] (
    [IdRedeSocial] BIGINT IDENTITY(1,1) NOT NULL,
    [Nome] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_RedeSocial] PRIMARY KEY ([IdRedeSocial])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Capa"                                                       */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Capa] (
    [IdCapa] BIGINT IDENTITY(1,1) NOT NULL,
    [Titulo] VARCHAR(500),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Width] INTEGER,
    [Height] INTEGER,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Capa] PRIMARY KEY ([IdCapa])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "CapaRedeSocial"                                             */
/* ---------------------------------------------------------------------- */
CREATE TABLE [CapaRedeSocial] (
    [IdCapaRedeSocial] BIGINT IDENTITY(1,1) NOT NULL,
    [IdRedeSocial] BIGINT,
    [IdCapa] BIGINT,
    [IdUsuario] BIGINT,
    [IdCampanha] BIGINT,
    CONSTRAINT [PK_CapaRedeSocial] PRIMARY KEY ([IdCapaRedeSocial])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AssinaturaCartao"                                           */
/* ---------------------------------------------------------------------- */
CREATE TABLE [AssinaturaCartao] (
    [IdAssinaturaCartao] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [IdCampanha] BIGINT,
    [Titulo] VARCHAR(500),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Width] INTEGER,
    [Height] INTEGER,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_AssinaturaCartao] PRIMARY KEY ([IdAssinaturaCartao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Banner"                                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Banner] (
    [IdBanner] BIGINT IDENTITY(1,1) NOT NULL,
    [IdInformacao] BIGINT,
    [IdCampanha] BIGINT,
    [IdUsuario] BIGINT,
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [Width] INTEGER,
    [Height] INTEGER,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Banner] PRIMARY KEY ([IdBanner])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Informacao"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Informacao] (
    [IdInformacao] BIGINT IDENTITY(1,1) NOT NULL,
    [Titulo] VARCHAR(500),
    [Descricao] VARCHAR(1000),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Informacao] PRIMARY KEY ([IdInformacao])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "GrupoCorretor"                                              */
/* ---------------------------------------------------------------------- */
CREATE TABLE [GrupoCorretor] (
    [IdGrupoCorretor] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_GrupoCorretor] PRIMARY KEY ([IdGrupoCorretor])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "GrupoCorretorCampanha"                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [GrupoCorretorCampanha] (
    [IdGrupoCorretorCampanha] BIGINT IDENTITY(1,1) NOT NULL,
    [IdGrupoCorretor] TINYINT,
    [IdCampanha] BIGINT,
    CONSTRAINT [PK_GrupoCorretorCampanha] PRIMARY KEY ([IdGrupoCorretorCampanha])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "GrupoCorretorOperadora"                                     */
/* ---------------------------------------------------------------------- */
CREATE TABLE [GrupoCorretorOperadora] (
    [IdGrupoCorretorOperadora] BIGINT IDENTITY(1,1) NOT NULL,
    [IdOperadora] BIGINT,
    [Individual] BIT NOT NULL,
    [Familiar] BIT NOT NULL,
    [Empresarial] BIT NOT NULL,
    [Vidas] INTEGER,
    [PossuiPlano] BIT NOT NULL,
    [PossuiCNPJ] BIT NOT NULL,
    [Nome] VARCHAR(200),
    [Email] VARCHAR(200),
    [Telefone] VARCHAR(10),
    [IP] VARCHAR(20),
    [DataNascimento] DATETIME NOT NULL,
    [Profissao] VARCHAR(40),
    [PossuiVeiculo] BIT NOT NULL,
    [BuscaVeiculo] BIT NOT NULL,
    [Casa] BIT NOT NULL,
    [Condominio] BIT NOT NULL,
    [Apartamento] BIT NOT NULL,
    [DataCadastro] TINYINT,
    CONSTRAINT [PK_GrupoCorretorOperadora] PRIMARY KEY ([IdGrupoCorretorOperadora])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "CalendarioSazonal"                                          */
/* ---------------------------------------------------------------------- */
CREATE TABLE [CalendarioSazonal] (
    [IdCalendarioSazonal] BIGINT IDENTITY(1,1) NOT NULL,
    [IdCalendario] BIGINT,
    [Descricao] VARCHAR(200),
    [Cor] VARCHAR(40),
    [Tipo] TINYINT,
    [GuidId] VARCHAR(255),
    [DataSazonal] DATETIME NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    [ExisteCampanha] BIT,
    [DataInicio] DATETIME,
    [DataFim] DATETIME,
    [Repete] TINYINT,
    [Frequencia] TINYINT,
    CONSTRAINT [PK_CalendarioSazonal] PRIMARY KEY ([IdCalendarioSazonal])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "VisitaCampanha"                                             */
/* ---------------------------------------------------------------------- */
CREATE TABLE [VisitaCampanha] (
    [IdVisitaCampanha] BIGINT NOT NULL,
    [IdVisita] BIGINT,
    [IdCampanha] BIGINT,
    CONSTRAINT [PK_VisitaCampanha] PRIMARY KEY ([IdVisitaCampanha])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "PropostaCliente"                                            */
/* ---------------------------------------------------------------------- */
CREATE TABLE [PropostaCliente] (
    [IdPropostaCliente] BIGINT IDENTITY(1,1) NOT NULL,
    [IdProposta] BIGINT NOT NULL,
    [IdCliente] BIGINT NOT NULL,
    [Dependente] BIT NOT NULL,
    [CompraCarencia] BIT,
    [VigenciaContrato] DATETIME,
    [NomePlano] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_PropostaCliente] PRIMARY KEY ([IdPropostaCliente])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Banco"                                                      */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Banco] (
    [IdBanco] BIGINT IDENTITY(1,1) NOT NULL,
    [Codigo] VARCHAR(10),
    [Nome] VARCHAR(500),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Banco] PRIMARY KEY ([IdBanco])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "LandingPage"                                                */
/* ---------------------------------------------------------------------- */
CREATE TABLE [LandingPage] (
    [IdLandingPage] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [Individual] BIT NOT NULL,
    [Familiar] BIT NOT NULL,
    [Empresarial] BIT NOT NULL,
    [Vidas] INTEGER,
    [PossuiPlano] BIT NOT NULL,
    [PossuiCNPJ] BIT NOT NULL,
    [Nome] VARCHAR(200),
    [Email] VARCHAR(200),
    [Outro] VARCHAR(200),
    [Telefone] VARCHAR(20),
    [IP] VARCHAR(20),
    [Profissao] VARCHAR(40),
    [PossuiVeiculo] BIT NOT NULL,
    [BuscaVeiculo] BIT NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_LandingPage] PRIMARY KEY ([IdLandingPage])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "LandingPageCarrossel"                                       */
/* ---------------------------------------------------------------------- */
CREATE TABLE [LandingPageCarrossel] (
    [IdLandingPageCarrossel] BIGINT IDENTITY(1,1) NOT NULL,
    [IdUsuario] BIGINT,
    [Titulo] VARCHAR(500),
    [CaminhoArquivo] VARCHAR(500),
    [NomeArquivo] VARCHAR(500),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_LandingPageCarrossel] PRIMARY KEY ([IdLandingPageCarrossel])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Fornecedor"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Fornecedor] (
    [IdFornecedor] TINYINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Usuario] VARCHAR(200),
    [Senha] VARCHAR(200),
    [TokenAPI] VARCHAR(200),
    [DataCadastro] DATETIME NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Fornecedor] PRIMARY KEY ([IdFornecedor])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "FornecedorConsulta"                                         */
/* ---------------------------------------------------------------------- */
CREATE TABLE [FornecedorConsulta] (
    [IdFornecedorConsulta] BIGINT IDENTITY(1,1) NOT NULL,
    [IdFornecedor] TINYINT,
    [IdUsuario] BIGINT,
    [Documento] VARCHAR(200),
    [Metodo] VARCHAR(200),
    [RetornoJson] VARCHAR(8000),
    [DataConsulta] DATETIME NOT NULL,
    [IP] VARCHAR(200),
    CONSTRAINT [PK_FornecedorConsulta] PRIMARY KEY ([IdFornecedorConsulta])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Calendario"                                                 */
/* ---------------------------------------------------------------------- */
CREATE TABLE [Calendario] (
    [IdCalendario] BIGINT IDENTITY(1,1) NOT NULL,
    [Descricao] VARCHAR(200),
    [Ativo] BIT NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    CONSTRAINT [PK_Calendario] PRIMARY KEY ([IdCalendario])
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

ALTER TABLE [Usuario] ADD CONSTRAINT [GrupoCorretor_Usuario] 
    FOREIGN KEY ([IdGrupoCorretor]) REFERENCES [GrupoCorretor] ([IdGrupoCorretor])
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

ALTER TABLE [Operadora] ADD CONSTRAINT [Modalidade_Operadora] 
    FOREIGN KEY ([IdModalidade]) REFERENCES [Modalidade] ([IdModalidade])
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

ALTER TABLE [Cliente] ADD CONSTRAINT [Cliente_Cliente] 
    FOREIGN KEY ([IdClienteReferencia]) REFERENCES [Cliente] ([IdCliente])
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

ALTER TABLE [Proposta] ADD CONSTRAINT [Banco_Proposta] 
    FOREIGN KEY ([IdBanco]) REFERENCES [Banco] ([IdBanco])
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

ALTER TABLE [KPIMeta] ADD CONSTRAINT [KPIGrupo_KPIMeta] 
    FOREIGN KEY ([IdKPIGrupo]) REFERENCES [KPIGrupo] ([IdKPIGrupo])
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

ALTER TABLE [Visita] ADD CONSTRAINT [CalendarioSazonal_Visita] 
    FOREIGN KEY ([IdCalendarioSazonal]) REFERENCES [CalendarioSazonal] ([IdCalendarioSazonal])
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

ALTER TABLE [KPIMetaValor] ADD CONSTRAINT [KPIMeta_KPIMetaValor] 
    FOREIGN KEY ([IdMeta]) REFERENCES [KPIMeta] ([IdMeta])
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

ALTER TABLE [CampanhaArquivo] ADD CONSTRAINT [Informacao_CampanhaArquivo] 
    FOREIGN KEY ([IdInformacao]) REFERENCES [Informacao] ([IdInformacao])
GO

ALTER TABLE [CampanhaArquivo] ADD CONSTRAINT [Calendario_CampanhaArquivo] 
    FOREIGN KEY ([IdCalendario]) REFERENCES [Calendario] ([IdCalendario])
GO

ALTER TABLE [KPIMetaVida] ADD CONSTRAINT [KPIMeta_KPIMetaVida] 
    FOREIGN KEY ([IdMeta]) REFERENCES [KPIMeta] ([IdMeta])
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

ALTER TABLE [KPIGrupo] ADD CONSTRAINT [Usuario_KPIGrupo] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [KPIGrupoUsuario] ADD CONSTRAINT [KPIGrupo_KPIGrupoUsuario] 
    FOREIGN KEY ([IdKPIGrupo]) REFERENCES [KPIGrupo] ([IdKPIGrupo])
GO

ALTER TABLE [KPIGrupoUsuario] ADD CONSTRAINT [Usuario_KPIGrupoUsuario] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [KPIGrupoUsuario] ADD CONSTRAINT [KPIMeta_KPIGrupoUsuario] 
    FOREIGN KEY ([IdMeta]) REFERENCES [KPIMeta] ([IdMeta])
GO

ALTER TABLE [Abordagem] ADD CONSTRAINT [Usuario_Abordagem] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Abordagem] ADD CONSTRAINT [AbordagemCategoria_Abordagem] 
    FOREIGN KEY ([IdAbordagemCategoria]) REFERENCES [AbordagemCategoria] ([IdAbordagemCategoria])
GO

ALTER TABLE [Video] ADD CONSTRAINT [Usuario_Video] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [Video] ADD CONSTRAINT [Campanha_Video] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [Video] ADD CONSTRAINT [Calendario_Video] 
    FOREIGN KEY ([IdCalendario]) REFERENCES [Calendario] ([IdCalendario])
GO

ALTER TABLE [CapaRedeSocial] ADD CONSTRAINT [RedeSocial_CapaRedeSocial] 
    FOREIGN KEY ([IdRedeSocial]) REFERENCES [RedeSocial] ([IdRedeSocial])
GO

ALTER TABLE [CapaRedeSocial] ADD CONSTRAINT [Capa_CapaRedeSocial] 
    FOREIGN KEY ([IdCapa]) REFERENCES [Capa] ([IdCapa])
GO

ALTER TABLE [CapaRedeSocial] ADD CONSTRAINT [Usuario_CapaRedeSocial] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [CapaRedeSocial] ADD CONSTRAINT [Campanha_CapaRedeSocial] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [AssinaturaCartao] ADD CONSTRAINT [Usuario_AssinaturaCartao] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [AssinaturaCartao] ADD CONSTRAINT [Campanha_AssinaturaCartao] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [Banner] ADD CONSTRAINT [Informacao_Banner] 
    FOREIGN KEY ([IdInformacao]) REFERENCES [Informacao] ([IdInformacao])
GO

ALTER TABLE [Banner] ADD CONSTRAINT [Campanha_Banner] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [Banner] ADD CONSTRAINT [Usuario_Banner] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [GrupoCorretorCampanha] ADD CONSTRAINT [GrupoCorretor_GrupoCorretorCampanha] 
    FOREIGN KEY ([IdGrupoCorretor]) REFERENCES [GrupoCorretor] ([IdGrupoCorretor])
GO

ALTER TABLE [GrupoCorretorCampanha] ADD CONSTRAINT [Campanha_GrupoCorretorCampanha] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [GrupoCorretorOperadora] ADD CONSTRAINT [Operadora_GrupoCorretorOperadora] 
    FOREIGN KEY ([IdOperadora]) REFERENCES [Operadora] ([IdOperadora])
GO

ALTER TABLE [GrupoCorretorOperadora] ADD CONSTRAINT [GrupoCorretor_GrupoCorretorOperadora] 
    FOREIGN KEY ([DataCadastro]) REFERENCES [GrupoCorretor] ([IdGrupoCorretor])
GO

ALTER TABLE [CalendarioSazonal] ADD CONSTRAINT [Calendario_CalendarioSazonal] 
    FOREIGN KEY ([IdCalendario]) REFERENCES [Calendario] ([IdCalendario])
GO

ALTER TABLE [VisitaCampanha] ADD CONSTRAINT [Visita_VisitaCampanha] 
    FOREIGN KEY ([IdVisita]) REFERENCES [Visita] ([IdVisita])
GO

ALTER TABLE [VisitaCampanha] ADD CONSTRAINT [Campanha_VisitaCampanha] 
    FOREIGN KEY ([IdCampanha]) REFERENCES [Campanha] ([IdCampanha])
GO

ALTER TABLE [PropostaCliente] ADD CONSTRAINT [Proposta_PropostaCliente] 
    FOREIGN KEY ([IdProposta]) REFERENCES [Proposta] ([IdProposta])
GO

ALTER TABLE [PropostaCliente] ADD CONSTRAINT [Cliente_PropostaCliente] 
    FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente])
GO

ALTER TABLE [LandingPage] ADD CONSTRAINT [Usuario_LandingPage] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [LandingPageCarrossel] ADD CONSTRAINT [Usuario_LandingPageCarrossel] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

ALTER TABLE [FornecedorConsulta] ADD CONSTRAINT [Fornecedor_FornecedorConsulta] 
    FOREIGN KEY ([IdFornecedor]) REFERENCES [Fornecedor] ([IdFornecedor])
GO

ALTER TABLE [FornecedorConsulta] ADD CONSTRAINT [Usuario_FornecedorConsulta] 
    FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([IdUsuario])
GO

