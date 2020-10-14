/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases v6.1.0                     */
/* Target DBMS:           MS SQL Server 2008                              */
/* Project file:          CRM_Model.dez                                   */
/* Project name:                                                          */
/* Author:                                                                */
/* Script type:           Database drop script                            */
/* Created on:            2020-10-13 21:27                                */
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

ALTER TABLE [Proposta] DROP CONSTRAINT [Produto_Proposta]
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

ALTER TABLE [Meta] DROP CONSTRAINT [Usuario_Meta]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [Proposta_Visita]
GO

ALTER TABLE [Visita] DROP CONSTRAINT [StatusVisita_Visita]
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

ALTER TABLE [KPIMeta] DROP CONSTRAINT [TipoLead_KPIMeta]
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
/* Drop table "PropostaFaixaEtaria"                                       */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [PropostaFaixaEtaria] DROP CONSTRAINT [PK_PropostaFaixaEtaria]
GO


/* Drop table */
DROP TABLE [PropostaFaixaEtaria]
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
/* Drop table "Visita"                                                    */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Visita] DROP CONSTRAINT [PK_Visita]
GO


/* Drop table */
DROP TABLE [Visita]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Meta"                                                      */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [Meta] DROP CONSTRAINT [PK_Meta]
GO


/* Drop table */
DROP TABLE [Meta]
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
/* Drop table "KPIMeta"                                                   */
/* ---------------------------------------------------------------------- */

/* Drop constraints */
ALTER TABLE [KPIMeta] DROP CONSTRAINT [PK_KPIMeta]
GO


/* Drop table */
DROP TABLE [KPIMeta]
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

