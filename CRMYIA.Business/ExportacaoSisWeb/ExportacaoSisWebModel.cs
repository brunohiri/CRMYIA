using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRMYIA.Business.Util;

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
                sbTextoProposta.AppendLine("PropostaNumero;PropostaDigito;CodigoEmpresa;Retroativa;IdModalidade;TxtModalidade;IdOperadora;TxtOperadora;IdSubProduto;TxtSubProduto;DtProducao;DtProtocolo;DtAssinatura;VigenciaManual;DtVigencia;DtVencimentoFixo;ImplantacaoManual;DtImplantacao;IdLoja;TxtLoja;IdCorretor;TxtCorretor;MultiNota;Segurado;Documento;TelComercial;TelResidencial;TelCelular;Email;CondicaoEspecial;VlProducao;QtdeBeneficiarios;PropostaColigadaPrincipalNumero;PropostaColigadaPrincipalQtdeBeneficiarios;ResponsavelEmpresaNome;ResponsavelFinanceiroNome;ResponsavelFinanceiroCPF;ResponsavelFinanceiroEmail;ResponsavelFinanceiroDtNascimento;IdProduto;TxtProduto;IdAdministradora;TxtAdministradora;IdEntidade;TxtEntidade;IdTabela;TxtTabela;IdPlano;TxtPlano;IdPlataforma;TxtPlataforma;IdVendedor;TxtVendedor;IdGrade;TxtGrade;IdAssistenteOriginal;TxtAssistenteOriginal;SupervisorNome;SupervisorCPF;SupervisorEmail;IdOrigem;TxtOrigem;IdConferente;TxtConferente;VlRepique;VlAdministrativo;VlBoleto;VlNet;PcDesconto;VlDescontoPrimeira;TaxaPaga;VlTaxa;TaxaParcela;IOFPago;VlIOF;IdFonte;TxtFonte;IdClassificacao;TxtClassificacao;GeraComissaoPrimeira;Integracao;Administrativa;Movimentacao;NaoParticipaCampanha;DentalIncluso;CoParticipacao;Acordo;AcompanharImplantacao;Portabilidade;Vip;Adaptacao;Regulamentacao;Obs");
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
                    sbTextoProposta.AppendFormat("{0};", "Não");//Retroativa
                    sbTextoProposta.AppendFormat("{0};", Item.IdModalidade.ToString());//IdModalidade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtModalidade
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora);//IdOperadora
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtOperadora
                    sbTextoProposta.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdProduto);//IdSubProduto
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtSubProduto
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy HH:mm"));//DtProducao
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy"));//DtProtocolo
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy HH:mm"));//DtAssinatura
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VigenciaManual
                    sbTextoProposta.AppendFormat("{0};", Item.DataSolicitacao?.ToString("dd/MM/yyyy"));//DtVigencia
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//DtVencimentoFixo
                    sbTextoProposta.AppendFormat("{0};", "Não");//ImplantacaoManual
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//DtImplantacao
                    sbTextoProposta.AppendFormat("{0};", "1");//IdLoja
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtLoja
                    sbTextoProposta.AppendFormat("{0};", Item.IdUsuarioCorretorNavigation?.IdUsuario);//IdCorretor
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtCorretor
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Multinota
                    sbTextoProposta.AppendFormat("{0};", Item.IdClienteNavigation?.Nome);//Segurado
                    sbTextoProposta.AppendFormat("{0};", Item.IdClienteNavigation?.CPF);//Documento
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TelComercial
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TelResidencial
                    sbTextoProposta.AppendFormat("{0};", Item.IdClienteNavigation?.Telefone.FirstOrDefault()?.Telefone1.KeepOnlyNumbersOrNull());//TelCelular
                    sbTextoProposta.AppendFormat("{0};", Item.IdClienteNavigation?.Email);//Email
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//CondicaoEspecial
                    sbTextoProposta.AppendFormat("{0};", (Item.ValorPrevisto.HasValue ? Item.ValorPrevisto.Value.ToString() : string.Empty));//VlProducao
                    sbTextoProposta.AppendFormat("{0};", Item.QuantidadeVidas);//QtdeBeneficiarios
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//PropostaColigadaPrincipalNumero
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//PropostaColigadaPrincipalQtdeBeneficiarios
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelEmpresaNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroCPF
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroEmail
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//ResponsavelFinanceiroDtNascimento
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdProduto
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtProduto
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdAdministradora
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtAdministradora
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdEntidade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtEntidade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdTabela
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtTabela
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdPlano
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtPlano
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdPlataforma
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtPlataforma
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdVendedor
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtVendedor
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdGrade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtGrade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdAssistenteOriginal
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtAssistenteOriginal
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorNome
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorCPF
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//SupervisorEmail
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdOrigem
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtOrigem
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdConferente
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtConferente
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlRepique
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlAdministrativo
                    sbTextoProposta.AppendFormat("{0};", (Item.ValorPrevisto.HasValue ? Item.ValorPrevisto.Value.ToString() : string.Empty));//VlBoleto
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlNet
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//PcDesconto
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlDescontoPrimeira
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TaxaPaga
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlTaxa
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TaxaParcela
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IOFPago
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//VlIOF
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdFonte
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtFonte
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//IdClassificacao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//TxtClassificacao
                    sbTextoProposta.AppendFormat("{0};", "Sim");//GeraComissaoPrimeira
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Integracao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Administrativa
                    sbTextoProposta.AppendFormat("{0};", "Sim");//Movimentacao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//NaoParticipaCampanha
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//DentalIncluso
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//CoParticipacao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Acordo
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//AcompanharImplantacao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Portabilidade
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Vip
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Adaptacao
                    sbTextoProposta.AppendFormat("{0};", string.Empty);//Regulamentacao
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
                    sbTextoBeneficiario.AppendFormat("{0};", "Não");//PreExistente
                    sbTextoBeneficiario.AppendFormat("{0};", "Não");//CarenciaOperadora
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
                    sbTextoParcela.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadora);//IdOperadora);
                    sbTextoParcela.AppendFormat("{0};", Item.IdCategoriaNavigation?.IdLinhaNavigation?.IdProdutoNavigation?.IdOperadoraNavigation?.Descricao);//TxtOperadora);
                    sbTextoParcela.AppendFormat("{0};", Item.IdModalidade.ToString());//IdModalidade
                    sbTextoParcela.AppendFormat("{0};", Item.IdModalidadeNavigation?.Descricao);//TxtModalidade
                    sbTextoParcela.AppendFormat("{0};", "1");//Parcela
                    sbTextoParcela.AppendFormat("{0};", "0");//Ordem
                    sbTextoParcela.AppendFormat("{0};", Item.ValorPrevisto?.ToString("c2"));//VlParcela
                    sbTextoParcela.AppendFormat("{0};", "0,2");//PcComissao
                    sbTextoParcela.AppendFormat("{0};", "Não");//Vitalicio
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
