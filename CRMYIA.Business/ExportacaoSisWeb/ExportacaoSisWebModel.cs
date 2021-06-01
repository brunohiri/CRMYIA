using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Business.ExportacaoSisWeb
{
    public class ExportacaoSisWebModel
    {
        #region Propriedades

        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static string GerarExportacaoSisWeb(List<Proposta> ListEntity)
        {
            StringBuilder sbTextoProposta = new StringBuilder();
            StringBuilder sbTextoBeneficiario = new StringBuilder();
            StringBuilder sbTextoParcela = new StringBuilder();

            string CaminhoArquivoZip = string.Format(@"C:\Temp\SisWeb\Proposta_{0:dd_MM_yyyy_HH_mm_ss}.zip", DateTime.Now);

            string CaminhoArquivoProposta = string.Format(@"C:\Temp\SisWeb\Proposta_{0:dd_MM_yyyy_HH_mm_ss}.csv", DateTime.Now);
            string CaminhoArquivoBeneficiario = string.Format(@"C:\Temp\SisWeb\Beneficiario_{0:dd_MM_yyyy_HH_mm_ss}.csv", DateTime.Now);
            string CaminhoArquivoParcela = string.Format(@"C:\Temp\SisWeb\Parcela_{0:dd_MM_yyyy_HH_mm_ss}.csv", DateTime.Now);
            try
            {
                #region Header Propostas
                sbTextoProposta.AppendLine("#LayoutImportacaoProposta");
                sbTextoProposta.AppendLine("PropostaNumero;PropostaDigito;CodigoEmpresa;Retroativa;IdModalidade;TxtModalidade;IdOperadora;TxtOperadora;IdSubProduto;TxtSubProduto;DtProducao;DtAssinatura;DtProtocolo;VigenciaManual;DtVigencia;ImplantacaoManual;DtImplantacao;IdLoja;TxtLoja;IdCorretor;TxtCorretor;MultiNota;Segurado;Documento;TelComercial;TelResidencial;TelCelular;Email;CondicaoEspecial;VlProducao;QtdeBeneficiarios;ResponsavelEmpresaNome;ResponsavelFinanceiroNome;ResponsavelFinanceiroCPF;ResponsavelFinanceiroEmail;ResponsavelFinanceiroDtNascimento;IdAssistenteOriginal;TxtAssistenteOriginal;IdVendedor;TxtVendedor;SupervisorNome;SupervisorCPF;SupervisorEmail;IdOrigem;TxtOrigem;IdConferente;TxtConferente;IdGrade;TxtGrade;VlRepique;VlAdministrativo;VlBoleto;VlNet;PcDesconto;VlDescontoPrimeira;TaxaPaga;VlTaxa;TaxaParcela;IOFPago;VlIOF;IdProduto;TxtProduto;IdAdministradora;TxtAdministradora;IdEntidade;TxtEntidade;IdTabela;TxtTabela;IdPlano;TxtPlano;IdPlataforma;TxtPlataforma;PropostaColigadaPrincipalNumero;PropostaColigadaPrincipalQtdeBeneficiarios;IdFonte;TxtFonte;IdClassificacao;TxtClassificacao;GeraComissaoPrimeira;Administrativa;Movimentacao;NaoParticipaCampanha;DentalIncluso;CoParticipacao;Acordo;AcompanharImplantacao;Portabilidade;Vip;Adaptacao;Regulamentacao;Obs");
                #endregion

                #region Header Beneficiários
                sbTextoBeneficiario.AppendLine("#LayoutImportacaoBeneficiario");
                sbTextoBeneficiario.AppendLine("PropostaNumero;IdOperadora;TxtOperadora;IdModalidade;TxtModalidade;Nome;CPF;RG;DtNascimento;Mae;IdBeneficiarioTipo;TxtBeneficiarioTipo;IdGrauParentesco;TxtGrauParentesco;IdSexo;TxtSexo;IdEstadoCivil;TxtEstadoCivil;IdPlano;TxtPlano;VlBruto;VlNET;PcDesconto;VlExtra;VlLiquido;ResponsavelNome;ResponsavelCPF;Carteirinha;TitularCPF;Peso;Altura;PreExistente;CarenciaOperadora;SUS;CNV;CEP;Logradouro;Numero;Complemento;Cidade;Bairro;UF");
                #endregion

                #region Header Parcelas
                sbTextoParcela.AppendLine("#LayoutImportacaoParcela");
                sbTextoParcela.AppendLine("PropostaNumero;IdOperadora;TxtOperadora;IdModalidade;TxtModalidade;Parcela;Ordem;VlParcela;PcComissao;Vitalicio;DtVencimento;IdComissionavel;TxtComissionavel;IdClassificacao;TxtClassificacao;VlTaxa");
                #endregion


                ListEntity.ForEach(delegate (Proposta Item)
                {
                    #region Proposta
                    sbTextoProposta.AppendFormat("{0};", Item.NumeroProposta);//PropostaNumero
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//PropostaDigito
                    sbTextoProposta.AppendFormat("{0};", string.Empty); //CodigoEmpresa
                    sbTextoProposta.AppendFormat("{0};", "false");//Retroativa
                    sbTextoProposta.AppendFormat("{0};", Item.IdModalidade.ToString());//IdModalidade
                    sbTextoProposta.AppendFormat("{0};", Item.IdModalidadeNavigation?.Descricao);//TxtModalidade
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora);//IdOperadora
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadoraNavigation?.Descricao);//TxtOperadora
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdProduto);//IdSubProduto
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.Descricao);//TxtSubProduto
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy HH:mm"));//DtProducao
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy"));//DtAssinatura
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy HH:mm"));//DtProtocolo
                    sbTextoProposta.AppendFormat("{0};", "True");//VigenciaManual
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy"));//DtVigencia
                    sbTextoProposta.AppendFormat("{0};", "False");//ImplantacaoManual
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//DtImplantacao
                    sbTextoProposta.AppendFormat("{0};", "1");//IdLoja
                    sbTextoProposta.AppendFormat("{0};", "Matriz - Campinas");//TxtLoja
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.IdUsuario);//IdCorretor
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.Nome);//TxtCorretor
                    sbTextoProposta.AppendFormat("{0};", "False");//Multinota
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Segurado
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.Documento);//Documento
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TelComercial
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TelResidencial
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.Telefone);//TelCelular
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.Email);//Email
                    sbTextoProposta.AppendFormat("{0};", "False");//CondicaoEspecial
                    sbTextoProposta.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlProducao
                    sbTextoProposta.AppendFormat("{0};", Item.QuantidadeVidas);//QtdeBeneficiarios
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelEmpresaNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroCPF
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroEmail
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroDtNascimento
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuario);//IdAssistenteOriginal
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioNavigation?.Nome);//TxtAssistenteOriginal
                    sbTextoProposta.AppendFormat("{0};", "-1");//IdVendedor
                    sbTextoProposta.AppendFormat("{0};", "Nenhum");//TxtVendedor
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorCPF
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorEmail
                    sbTextoProposta.AppendFormat("{0};", "-1");//IdOrigem
                    sbTextoProposta.AppendFormat("{0};", "Nenhum");//TxtOrigem
                    sbTextoProposta.AppendFormat("{0};", "-1");//IdConferente
                    sbTextoProposta.AppendFormat("{0};", "Nenhum");//TxtConferente
                    sbTextoProposta.AppendFormat("{0};", "4");//IdGrade
                    sbTextoProposta.AppendFormat("{0};", "EQUIPE DEDICADA");//TxtGrade
                    sbTextoProposta.AppendFormat("{0};", "0");//VlRepique
                    sbTextoProposta.AppendFormat("{0};", "0");//VlAdministrativo
                    sbTextoProposta.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlBoleto
                    sbTextoProposta.AppendFormat("{0};", "0");//VlNet
                    sbTextoProposta.AppendFormat("{0};", "0");//PcDesconto
                    sbTextoProposta.AppendFormat("{0};", "0");//VlDescontoPrimeira
                    sbTextoProposta.AppendFormat("{0};", "False");//TaxaPaga
                    sbTextoProposta.AppendFormat("{0};", "0");//VlTaxa
                    sbTextoProposta.AppendFormat("{0};", "0");//TaxaParcela
                    sbTextoProposta.AppendFormat("{0};", "False");//IOFPago
                    sbTextoProposta.AppendFormat("{0};", "0");//VlIOF
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdProduto);//IdProduto
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.Descricao);//TxtProduto
                    sbTextoProposta.AppendFormat("{0};", "0");//IdAdministradora
                    sbTextoProposta.AppendFormat("{0};", "NENHUMA");//TxtAdministradora
                    sbTextoProposta.AppendFormat("{0};", "0");//IdEntidade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtEntidade
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdLinha);//IdTabela
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.Descricao);//TxtTabela
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdCategoria);//IdPlano
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.Descricao);//TxtPlano
                    sbTextoProposta.AppendFormat("{0};", "1000");//IdPlataforma
                    sbTextoProposta.AppendFormat("{0};", "CRM YIA");//TxtPlataforma
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//PropostaColigadaPrincipalNumero
                    sbTextoProposta.AppendFormat("{0};", Item.QuantidadeVidas);//PropostaColigadaPrincipalQtdeBeneficiarios
                    sbTextoProposta.AppendFormat("{0};", "-1");//IdFonte
                    sbTextoProposta.AppendFormat("{0};", "Nenhum");//TxtFonte
                    sbTextoProposta.AppendFormat("{0};", "-1");//IdClassificacao
                    sbTextoProposta.AppendFormat("{0};", "Nenhuma");//TxtClassificacao
                    sbTextoProposta.AppendFormat("{0};", "True");//GeraComissaoPrimeira
                    sbTextoProposta.AppendFormat("{0};", "False");//Administrativa
                    sbTextoProposta.AppendFormat("{0};", "False");//Movimentacao
                    sbTextoProposta.AppendFormat("{0};", "False");//NaoParticipaCampanha
                    sbTextoProposta.AppendFormat("{0};", "False");//DentalIncluso
                    sbTextoProposta.AppendFormat("{0};", "False");//CoParticipacao
                    sbTextoProposta.AppendFormat("{0};", "False");//Acordo
                    sbTextoProposta.AppendFormat("{0};", "False");//AcompanharImplantacao
                    sbTextoProposta.AppendFormat("{0};", "False");//Portabilidade
                    sbTextoProposta.AppendFormat("{0};", "False");//Vip
                    sbTextoProposta.AppendFormat("{0};", "False");//Adaptacao
                    sbTextoProposta.AppendFormat("{0};", "False");//Regulamentacao
                    sbTextoProposta.AppendFormat("{0}\n", Item.Observacoes);//Obs
                    #endregion

                    #region Beneficiário
                    sbTextoBeneficiario.AppendFormat("{0};", Item.NumeroProposta);//PropostaNumero
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdModalidade.ToString());//IdModalidade
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdModalidadeNavigation?.Descricao);//TxtModalidade
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora);//IdOperadora
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadoraNavigation?.Descricao);//TxtOperadora
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Nome);//Nome
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.CPF);//CPF
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.RG);//RG
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.DataNascimento?.ToString("dd/MM/yyyy"));//DtNascimento
                    sbTextoBeneficiario.AppendFormat("{0};", string.Empty);//Mae
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//IdBeneficiarioTipo
                    sbTextoBeneficiario.AppendFormat("{0};", "Producao");//TxtBeneficiarioTipo
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Titular ? "31" : "18");//IdGrauParentesco
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Titular ? "Titular" : "Cônjuge");//TxtGrauParentesco
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation?.IdGeneroNavigation?.IdGenero);//IdSexo
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation?.IdGeneroNavigation?.Descricao);//TxtSexo
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation?.IdEstadoCivilNavigation?.IdEstadoCivil);//IdEstadoCivil
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation?.IdEstadoCivilNavigation?.Descricao);//TxtEstadoCivil
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdCategoria);//IdPlano
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdCategoriaNavigation?.Descricao);//TxtPlano
                    sbTextoBeneficiario.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlBruto
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//VlNet
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//PcDesconto
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//VlExtra
                    sbTextoBeneficiario.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlLiquido
                    sbTextoBeneficiario.AppendFormat("{0};", string.Empty);//ResponsavelNome
                    sbTextoBeneficiario.AppendFormat("{0};", string.Empty);//ResponsavelCPF
                    sbTextoBeneficiario.AppendFormat("{0};", string.Empty);//Carteirinha
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.CPF);//TitularCPF
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//Peso
                    sbTextoBeneficiario.AppendFormat("{0};", "0");//Altura
                    sbTextoBeneficiario.AppendFormat("{0};", "False");//PreExistente
                    sbTextoBeneficiario.AppendFormat("{0};", "False");//CarenciaOperadora
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.CartaoSus);//SUS
                    sbTextoBeneficiario.AppendFormat("{0};", string.Empty);//CNV
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.CEP);//CEP
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Endereco);//Logradouro
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Numero);//Numero
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Complemento);//Complemento
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.IdCidadeNavigation?.Descricao);//Cidade
                    sbTextoBeneficiario.AppendFormat("{0};", Item.IdClienteNavigation.Bairro);//Bairro
                    sbTextoBeneficiario.AppendFormat("{0}\n", Item.IdClienteNavigation.IdCidadeNavigation?.IdEstadoNavigation?.Sigla);//UF

                    #endregion

                    #region Parcelas

                    sbTextoParcela.AppendFormat("{0};", Item.NumeroProposta);//PropostaNumero
                    sbTextoParcela.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora);//IdOperadora);//IdOperadora
                    sbTextoParcela.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadoraNavigation?.Descricao);//TxtOperadora);//TxtOperadora
                    sbTextoParcela.AppendFormat("{0};", Item.IdModalidade.ToString());//IdModalidade
                    sbTextoParcela.AppendFormat("{0};", Item.IdModalidadeNavigation?.Descricao);//TxtModalidade
                    sbTextoParcela.AppendFormat("{0};", "1");//Parcela
                    sbTextoParcela.AppendFormat("{0};", "0");//Ordem
                    sbTextoParcela.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlParcela
                    sbTextoParcela.AppendFormat("{0};", "0,2");//PcComissao
                    sbTextoParcela.AppendFormat("{0};", "False");//Vitalicio
                    sbTextoParcela.AppendFormat("{0};", Item.DataSolicitacao.Value.AddMonths(1).ToString("dd/MM/yyyy"));//DtVencimento
                    sbTextoParcela.AppendFormat("{0};", string.Empty);//IdComissionavel
                    sbTextoParcela.AppendFormat("{0};", string.Empty);//TxtComissionavel
                    sbTextoParcela.AppendFormat("{0};", "0");//IdClassificacao
                    sbTextoParcela.AppendFormat("{0};", "Nenhum");//TxtClassificacao
                    sbTextoParcela.AppendFormat("{0}\n", "0");//VlTaxa

                    #endregion
                });

                if (!System.IO.Directory.Exists(@"C:\Temp\SisWeb"))
                    System.IO.Directory.CreateDirectory(@"C:\Temp\SisWeb");

                System.IO.File.WriteAllText(CaminhoArquivoProposta, sbTextoProposta.ToString(), Encoding.UTF8);
                System.IO.File.WriteAllText(CaminhoArquivoBeneficiario, sbTextoBeneficiario.ToString(), Encoding.UTF8);
                System.IO.File.WriteAllText(CaminhoArquivoParcela, sbTextoParcela.ToString(), Encoding.UTF8);

                string[] CaminhoArquivos = { CaminhoArquivoProposta, CaminhoArquivoBeneficiario, CaminhoArquivoParcela };
                Util.Util.CompactarArquivo(CaminhoArquivoZip, CaminhoArquivos);

            }
            catch (Exception)
            {

                throw;
            }
            return CaminhoArquivoZip;
        }
        #endregion
    }
}
