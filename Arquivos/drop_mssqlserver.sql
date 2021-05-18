/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.1.0                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          CRM_Model.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2021-05-13 10:42                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */
ALTER TABLE [Usuario] DROP CONSTRAINT [Corretora_Usuario]
GO

ALTER TABLE [Usuario] DROP CONSTRAINT [Classificacao_Usuario]
GO

ALTER TABLE [Usuario] DROP CONSTRAINT [Producao_Usuario]
GO

ALTER TABLE [Usuario] DROP CONSTRAINT [GrupoCorretor_Usuario]
GO

ALTER TABLE [HistoricoAcesso] DROP CONSTRAINT [Usuario_HistoricoAcesso]
GO

ALTER TABLE [UsuarioHierarquia] DROP CONSTRAINT [Usuario_UsuarioHierarquiaMasterMaster]
GO

ALTER TABLE [UsuarioHierarquia] DROP CONSTRAINT [Usuario_UsuarioHierarquiaSlaveSlave]
GO

ALTER TABLE [Produto] DROP CONSTRAINT [Operadora_Produto]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [Cidade_Cliente]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [EstadoCivil_Cliente]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [Genero_Cliente]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [Origem_Cliente]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [TipoLead_Cliente]
GO

ALTER TABLE [Cliente] DROP CONSTRAINT [ArquivoLead_Cliente]
GO

ALTER TABLE [Cidade] DROP CONSTRAINT [Estado_Cidade]
GO

ALTER TABLE [Telefone] DROP CONSTRAINT [OperadoraTelefone_Telefone]
GO

ALTER TABLE [Telefone] DROP CONSTRAINT [Cliente_Telefone]
GO

ALTER TABLE [Email] DROP CONSTRAINT [Cliente_Email]
GO

ALTER TABLE [UsuarioCliente] DROP CONSTRAINT [Usuario_UsuarioCliente]
GO

ALTER TABLE [UsuarioCliente] DROP CONSTRAINT [Cliente_UsuarioCliente]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Modalidade_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Cliente_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Usuario_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [StatusProposta_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [MotivoDeclinio_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [FaseProposta_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [UsuarioCorretor_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Categoria_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Porte_Proposta]
GO

ALTER TABLE [Proposta] DROP CONSTRAINT [Banco_Proposta]
GO

ALTER TABLE [HistoricoProposta] DROP CONSTRAINT [Proposta_HistoricoProposta]
GO

ALTER TABLE [HistoricoProposta] DROP CONSTRAINT [Usuario_HistoricoProposta]
GO

ALTER TABLE [Documento] DROP CONSTRAINT [Proposta_Documento]
GO

ALTER TABLE [Documento] DROP CONSTRAINT [TipoDocumento_Documento]
GO

ALTER TABLE [OperadoraDocumento] DROP CONSTRAINT [Operadora_OperadoraDocumento]
GO

ALTER TABLE [OperadoraDocumento] DROP CONSTRAINT [TipoDocumento_OperadoraDocumento]
GO

ALTER TABLE [KPIMeta] DROP CONSTRAINT [KPIGrupo_KPIMeta]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [Proposta_Visita]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [StatusVisita_Visita]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [Usuario_Visita]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [CalendarioSazonal_Visita]
GO

ALTER TABLE [Corretora] DROP CONSTRAINT [Cidade_Corretora]
GO

ALTER TABLE [Modulo] DROP CONSTRAINT [Modulo_Modulo]
GO

ALTER TABLE [PerfilModulo] DROP CONSTRAINT [Modulo_PerfilModulo]
GO

ALTER TABLE [PerfilModulo] DROP CONSTRAINT [Perfil_PerfilModulo]
GO

ALTER TABLE [UsuarioPerfil] DROP CONSTRAINT [Perfil_UsuarioPerfil]
GO

ALTER TABLE [UsuarioPerfil] DROP CONSTRAINT [Usuario_UsuarioPerfil]
GO

ALTER TABLE [PropostaFaixaEtaria] DROP CONSTRAINT [Proposta_PropostaFaixaEtaria]
GO

ALTER TABLE [PropostaFaixaEtaria] DROP CONSTRAINT [FaixaEtaria_PropostaFaixaEtaria]
GO

ALTER TABLE [KPIMetaValor] DROP CONSTRAINT [KPIMeta_KPIMetaValor]
GO

ALTER TABLE [Linha] DROP CONSTRAINT [Produto_Linha]
GO

ALTER TABLE [Categoria] DROP CONSTRAINT [Linha_Categoria]
GO

ALTER TABLE [Notificacao] DROP CONSTRAINT [Usuario_Notificacao_Cadastro]
GO

ALTER TABLE [Notificacao] DROP CONSTRAINT [Usuario_Notificacao_Visualizar]
GO

ALTER TABLE [HistoricoLigacao] DROP CONSTRAINT [Proposta_HistoricoLigacao]
GO

ALTER TABLE [HistoricoLigacao] DROP CONSTRAINT [Usuario_HistoricoLigacao]
GO

ALTER TABLE [Campanha] DROP CONSTRAINT [Usuario_Campanha]
GO

ALTER TABLE [Campanha] DROP CONSTRAINT [CalendarioSazonal_Campanha]
GO

ALTER TABLE [CampanhaArquivo] DROP CONSTRAINT [Campanha_CampanhaArquivo]
GO

ALTER TABLE [CampanhaArquivo] DROP CONSTRAINT [Informacao_CampanhaArquivo]
GO

ALTER TABLE [KPIMetaVida] DROP CONSTRAINT [KPIMeta_KPIMetaVida]
GO

ALTER TABLE [Chat] DROP CONSTRAINT [Usuario_Chat_De]
GO

ALTER TABLE [Chat] DROP CONSTRAINT [Usuario_Chat_Para]
GO

ALTER TABLE [NotificacaoMensagem] DROP CONSTRAINT [Usuario_NotificacaoMensagem_De]
GO

ALTER TABLE [NotificacaoMensagem] DROP CONSTRAINT [Usuario_NotificacaoMensagem_Para]
GO

ALTER TABLE [KPIGrupo] DROP CONSTRAINT [Usuario_KPIGrupo]
GO

ALTER TABLE [KPIGrupoUsuario] DROP CONSTRAINT [KPIGrupo_KPIGrupoUsuario]
GO

ALTER TABLE [KPIGrupoUsuario] DROP CONSTRAINT [Usuario_KPIGrupoUsuario]
GO

ALTER TABLE [KPIGrupoUsuario] DROP CONSTRAINT [KPIMeta_KPIGrupoUsuario]
GO

ALTER TABLE [Abordagem] DROP CONSTRAINT [Usuario_Abordagem]
GO

ALTER TABLE [Abordagem] DROP CONSTRAINT [AbordagemCategoria_Abordagem]
GO

ALTER TABLE [Video] DROP CONSTRAINT [Usuario_Video]
GO

ALTER TABLE [Video] DROP CONSTRAINT [Campanha_Video]
GO

ALTER TABLE [CapaRedeSocial] DROP CONSTRAINT [RedeSocial_CapaRedeSocial]
GO

ALTER TABLE [CapaRedeSocial] DROP CONSTRAINT [Capa_CapaRedeSocial]
GO

ALTER TABLE [CapaRedeSocial] DROP CONSTRAINT [Usuario_CapaRedeSocial]
GO

ALTER TABLE [CapaRedeSocial] DROP CONSTRAINT [Campanha_CapaRedeSocial]
GO

ALTER TABLE [AssinaturaCartao] DROP CONSTRAINT [Usuario_AssinaturaCartao]
GO

ALTER TABLE [AssinaturaCartao] DROP CONSTRAINT [Campanha_AssinaturaCartao]
GO

ALTER TABLE [Banner] DROP CONSTRAINT [Informacao_Banner]
GO

ALTER TABLE [Banner] DROP CONSTRAINT [Campanha_Banner]
GO

ALTER TABLE [Banner] DROP CONSTRAINT [Usuario_Banner]
GO

ALTER TABLE [GrupoCorretorCampanha] DROP CONSTRAINT [GrupoCorretor_GrupoCorretorCampanha]
GO

ALTER TABLE [GrupoCorretorCampanha] DROP CONSTRAINT [Campanha_GrupoCorretorCampanha]
GO

ALTER TABLE [GrupoCorretorOperadora] DROP CONSTRAINT [Operadora_GrupoCorretorOperadora]
GO

ALTER TABLE [GrupoCorretorOperadora] DROP CONSTRAINT [GrupoCorretor_GrupoCorretorOperadora]
GO

ALTER TABLE [VisitaCampanha] DROP CONSTRAINT [Visita_VisitaCampanha]
GO

ALTER TABLE [VisitaCampanha] DROP CONSTRAINT [Campanha_VisitaCampanha]
GO

ALTER TABLE [PropostaCliente] DROP CONSTRAINT [Proposta_PropostaCliente]
GO

ALTER TABLE [PropostaCliente] DROP CONSTRAINT [Cliente_PropostaCliente]
GO

ALTER TABLE [LandingPage] DROP CONSTRAINT [Usuario_LandingPage]
GO

ALTER TABLE [LandingPageCarrossel] DROP CONSTRAINT [Usuario_LandingPageCarrossel]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PerfilModulo"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [PerfilModulo] DROP CONSTRAINT [PK_PerfilModulo]
GO


/* Drop table */
DROP TABLE [PerfilModulo]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Modulo"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Modulo] DROP CONSTRAINT [PK_Modulo]
GO


/* Drop table */
DROP TABLE [Modulo]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PropostaCliente"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [PropostaCliente] DROP CONSTRAINT [PK_PropostaCliente]
GO


/* Drop table */
DROP TABLE [PropostaCliente]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "VisitaCampanha"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [VisitaCampanha] DROP CONSTRAINT [PK_VisitaCampanha]
GO


/* Drop table */
DROP TABLE [VisitaCampanha]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KPIGrupoUsuario"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIGrupoUsuario] DROP CONSTRAINT [PK_KPIGrupoUsuario]
GO


/* Drop table */
DROP TABLE [KPIGrupoUsuario]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KPIMetaVida"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIMetaVida] DROP CONSTRAINT [PK_KPIMetaVida]
GO


/* Drop table */
DROP TABLE [KPIMetaVida]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "HistoricoLigacao"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [HistoricoLigacao] DROP CONSTRAINT [PK_HistoricoLigacao]
GO


/* Drop table */
DROP TABLE [HistoricoLigacao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KPIMetaValor"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIMetaValor] DROP CONSTRAINT [PK_KPIMetaValor]
GO


/* Drop table */
DROP TABLE [KPIMetaValor]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "PropostaFaixaEtaria"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [PropostaFaixaEtaria] DROP CONSTRAINT [PK_PropostaFaixaEtaria]
GO


/* Drop table */
DROP TABLE [PropostaFaixaEtaria]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Visita"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Visita] DROP CONSTRAINT [PK_Visita]
GO


/* Drop table */
DROP TABLE [Visita]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KPIMeta"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIMeta] DROP CONSTRAINT [PK_KPIMeta]
GO


/* Drop table */
DROP TABLE [KPIMeta]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Documento"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Documento] DROP CONSTRAINT [PK_Documento]
GO


/* Drop table */
DROP TABLE [Documento]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "HistoricoProposta"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [HistoricoProposta] DROP CONSTRAINT [PK_HistoricoProposta]
GO


/* Drop table */
DROP TABLE [HistoricoProposta]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Proposta"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Proposta] DROP CONSTRAINT [PK_Proposta]
GO


/* Drop table */
DROP TABLE [Proposta]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "LandingPageCarrossel"                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [LandingPageCarrossel] DROP CONSTRAINT [PK_LandingPageCarrossel]
GO


/* Drop table */
DROP TABLE [LandingPageCarrossel]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "LandingPage"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [LandingPage] DROP CONSTRAINT [PK_LandingPage]
GO


/* Drop table */
DROP TABLE [LandingPage]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "GrupoCorretorCampanha"                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [GrupoCorretorCampanha] DROP CONSTRAINT [PK_GrupoCorretorCampanha]
GO


/* Drop table */
DROP TABLE [GrupoCorretorCampanha]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Banner"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Banner] DROP CONSTRAINT [PK_Banner]
GO


/* Drop table */
DROP TABLE [Banner]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AssinaturaCartao"                                          */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [AssinaturaCartao] DROP CONSTRAINT [PK_AssinaturaCartao]
GO


/* Drop table */
DROP TABLE [AssinaturaCartao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "CapaRedeSocial"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [CapaRedeSocial] DROP CONSTRAINT [PK_CapaRedeSocial]
GO


/* Drop table */
DROP TABLE [CapaRedeSocial]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Video"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

/* Drop table */
DROP TABLE [Video]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Abordagem"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Abordagem] DROP CONSTRAINT [PK_Abordagem]
GO


/* Drop table */
DROP TABLE [Abordagem]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "KPIGrupo"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIGrupo] DROP CONSTRAINT [PK_KPIGrupo]
GO


/* Drop table */
DROP TABLE [KPIGrupo]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "NotificacaoMensagem"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */

/* Drop table */
DROP TABLE [NotificacaoMensagem]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Chat"                                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Chat] DROP CONSTRAINT [PK_Chat]
GO


/* Drop table */
DROP TABLE [Chat]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "CampanhaArquivo"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [CampanhaArquivo] DROP CONSTRAINT [PK_CampanhaArquivo]
GO


/* Drop table */
DROP TABLE [CampanhaArquivo]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Campanha"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Campanha] DROP CONSTRAINT [PK_Campanha]
GO


/* Drop table */
DROP TABLE [Campanha]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Notificacao"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Notificacao] DROP CONSTRAINT [PK_Notificacao]
GO


/* Drop table */
DROP TABLE [Notificacao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Categoria"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Categoria] DROP CONSTRAINT [PK_Categoria]
GO


/* Drop table */
DROP TABLE [Categoria]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Linha"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Linha] DROP CONSTRAINT [PK_Linha]
GO


/* Drop table */
DROP TABLE [Linha]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "UsuarioPerfil"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [UsuarioPerfil] DROP CONSTRAINT [PK_UsuarioPerfil]
GO


/* Drop table */
DROP TABLE [UsuarioPerfil]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "UsuarioCliente"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [UsuarioCliente] DROP CONSTRAINT [PK_UsuarioCliente]
GO


/* Drop table */
DROP TABLE [UsuarioCliente]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Email"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Email] DROP CONSTRAINT [PK_Email]
GO


/* Drop table */
DROP TABLE [Email]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Telefone"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Telefone] DROP CONSTRAINT [PK_Telefone]
GO


/* Drop table */
DROP TABLE [Telefone]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Cliente"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Cliente] DROP CONSTRAINT [PK_Cliente]
GO


/* Drop table */
DROP TABLE [Cliente]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Produto"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Produto] DROP CONSTRAINT [PK_Produto]
GO


/* Drop table */
DROP TABLE [Produto]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "UsuarioHierarquia"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [UsuarioHierarquia] DROP CONSTRAINT [PK_UsuarioHierarquia]
GO


/* Drop table */
DROP TABLE [UsuarioHierarquia]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "HistoricoAcesso"                                           */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [HistoricoAcesso] DROP CONSTRAINT [PK_HistoricoAcesso]
GO


/* Drop table */
DROP TABLE [HistoricoAcesso]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Usuario"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Usuario] DROP CONSTRAINT [PK_Usuario]
GO


/* Drop table */
DROP TABLE [Usuario]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Banco"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Banco] DROP CONSTRAINT [PK_Banco]
GO


/* Drop table */
DROP TABLE [Banco]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "CalendarioSazonal"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [CalendarioSazonal] DROP CONSTRAINT [PK_CalendarioSazonal]
GO


/* Drop table */
DROP TABLE [CalendarioSazonal]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "GrupoCorretorOperadora"                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [GrupoCorretorOperadora] DROP CONSTRAINT [PK_GrupoCorretorOperadora]
GO


/* Drop table */
DROP TABLE [GrupoCorretorOperadora]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "GrupoCorretor"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [GrupoCorretor] DROP CONSTRAINT [PK_GrupoCorretor]
GO


/* Drop table */
DROP TABLE [GrupoCorretor]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Informacao"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Informacao] DROP CONSTRAINT [PK_Informacao]
GO


/* Drop table */
DROP TABLE [Informacao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Capa"                                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Capa] DROP CONSTRAINT [PK_Capa]
GO


/* Drop table */
DROP TABLE [Capa]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "RedeSocial"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [RedeSocial] DROP CONSTRAINT [PK_RedeSocial]
GO


/* Drop table */
DROP TABLE [RedeSocial]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "AbordagemCategoria"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [AbordagemCategoria] DROP CONSTRAINT [PK_AbordagemCategoria]
GO


/* Drop table */
DROP TABLE [AbordagemCategoria]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "ArquivoLead"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [ArquivoLead] DROP CONSTRAINT [PK_ArquivoLead]
GO


/* Drop table */
DROP TABLE [ArquivoLead]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Porte"                                                     */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Porte] DROP CONSTRAINT [PK_Porte]
GO


/* Drop table */
DROP TABLE [Porte]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Producao"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Producao] DROP CONSTRAINT [PK_Producao]
GO


/* Drop table */
DROP TABLE [Producao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Classificacao"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Classificacao] DROP CONSTRAINT [PK_Classificacao]
GO


/* Drop table */
DROP TABLE [Classificacao]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "FaixaEtaria"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [FaixaEtaria] DROP CONSTRAINT [PK_FaixaEtaria]
GO


/* Drop table */
DROP TABLE [FaixaEtaria]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Corretora"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Corretora] DROP CONSTRAINT [PK_Corretora]
GO


/* Drop table */
DROP TABLE [Corretora]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusVisita"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [StatusVisita] DROP CONSTRAINT [PK_StatusVisita]
GO


/* Drop table */
DROP TABLE [StatusVisita]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "OperadoraDocumento"                                        */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [OperadoraDocumento] DROP CONSTRAINT [PK_OperadoraDocumento]
GO


/* Drop table */
DROP TABLE [OperadoraDocumento]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "TipoDocumento"                                             */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [TipoDocumento] DROP CONSTRAINT [PK_TipoDocumento]
GO


/* Drop table */
DROP TABLE [TipoDocumento]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "FaseProposta"                                              */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [FaseProposta] DROP CONSTRAINT [PK_FaseProposta]
GO


/* Drop table */
DROP TABLE [FaseProposta]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "TipoLead"                                                  */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [TipoLead] DROP CONSTRAINT [PK_TipoLead]
GO


/* Drop table */
DROP TABLE [TipoLead]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "MotivoDeclinio"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [MotivoDeclinio] DROP CONSTRAINT [PK_MotivoDeclinio]
GO


/* Drop table */
DROP TABLE [MotivoDeclinio]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "StatusProposta"                                            */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [StatusProposta] DROP CONSTRAINT [PK_StatusProposta]
GO


/* Drop table */
DROP TABLE [StatusProposta]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Modalidade"                                                */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Modalidade] DROP CONSTRAINT [PK_Modalidade]
GO


/* Drop table */
DROP TABLE [Modalidade]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Origem"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Origem] DROP CONSTRAINT [PK_Origem]
GO


/* Drop table */
DROP TABLE [Origem]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Genero"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Genero] DROP CONSTRAINT [PK_Genero]
GO


/* Drop table */
DROP TABLE [Genero]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "EstadoCivil"                                               */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [EstadoCivil] DROP CONSTRAINT [PK_EstadoCivil]
GO


/* Drop table */
DROP TABLE [EstadoCivil]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "OperadoraTelefone"                                         */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [OperadoraTelefone] DROP CONSTRAINT [PK_OperadoraTelefone]
GO


/* Drop table */
DROP TABLE [OperadoraTelefone]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Cidade"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Cidade] DROP CONSTRAINT [PK_Cidade]
GO


/* Drop table */
DROP TABLE [Cidade]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Estado"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Estado] DROP CONSTRAINT [PK_Estado]
GO


/* Drop table */
DROP TABLE [Estado]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Operadora"                                                 */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Operadora] DROP CONSTRAINT [PK_Operadora]
GO


/* Drop table */
DROP TABLE [Operadora]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Perfil"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Perfil] DROP CONSTRAINT [PK_Perfil]
GO


/* Drop table */
DROP TABLE [Perfil]
GO

