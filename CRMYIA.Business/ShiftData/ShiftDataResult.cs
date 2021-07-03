using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Business.ShiftData
{
    class ShiftDataResult
    {
    }

    public class ShiftDataResultLogin
    {
        public bool authenticated { get; set; }
        public string created { get; set; }
        public string expiration { get; set; }
        public string accessToken { get; set; }
        public string message { get; set; }
    }

    public class ShiftDataResultApiPessoaFisica
    {
        public int Code { get; set; }
        public string CodeMessage { get; set; }
        public string Message { get; set; }
        public int ElapsedTimeInMilliseconds { get; set; }
        public DateTime DateTimeExecution { get; set; }
        public string ApiVersion { get; set; }
        public ShiftDataResultPessoaFisica Result { get; set; }
    }

    public class ShiftDataResultPessoaFisica
    {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public string PrimeiroNome { get; set; }
        public string NomeMeio { get; set; }
        public string UltimoNome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }
        public string EstadoCivil { get; set; }
        public string Escolaridade { get; set; }
        public int? Dependentes { get; set; }
        public string Nacionalidade { get; set; }
        public string CodigoCbo { get; set; }
        public string DescricaoCbo { get; set; }
        public string Sexo { get; set; }
        public string Signo { get; set; }
        public long? Renda { get; set; }

        public List<ShiftDataResultEndereco> Enderecos { get; set; }

        public List<ShiftDataResultTelefone> Telefones { get; set; }

        public List<ShiftDataResultEmail> Emails { get; set; }
    }

    public class ShiftDataResultApiPessoaJuridica
    {
        public int Code { get; set; }
        public string CodeMessage { get; set; }
        public string Message { get; set; }
        public int ElapsedTimeInMilliseconds { get; set; }
        public DateTime DateTimeExecution { get; set; }
        public string ApiVersion { get; set; }
        public ShiftDataResultPessoaJuridica Result { get; set; }
    }

    public class ShiftDataResultPessoaJuridica
    {
        public string CNPJ { get; set; }
        public string NomeRazao { get; set; }
        public string NomeFantasia { get; set; }
        public bool? Matriz { get; set; }
        public DateTime? DataAbertura { get; set; }
        public int? Idade { get; set; }
        public string CodigoCnae { get; set; }
        public string DescricaoCnae { get; set; }
        public string CodigoNaturezaJuridica { get; set; }
        public string DescricaoNaturezaJuridica { get; set; }
        public decimal? CapitalSocial { get; set; }
        public string CodigoSituacaoCadastral { get; set; }
        public string DescricaoSituacaoCadastral { get; set; }
        public DateTime? DataSituacaoCadastral { get; set; }
        public DateTime? DataConsultaSituacaoCadastral { get; set; }
        public string HoraConsultaSituacaoCadastral { get; set; }
        public string MotivoSituacaoCadastral { get; set; }
        public string SituacaoEspecial { get; set; }
        public DateTime? DataSituacaoEspecial { get; set; }
        public string FaixaFaturamento { get; set; }
        public string FaixaFuncionario { get; set; }
        public string Porte { get; set; }
        public string Tipo { get; set; }

        public List<ShiftDataResultEndereco> Enderecos { get; set; }

        public List<ShiftDataResultTelefone> Telefones { get; set; }

        public List<ShiftDataResultEmail> Emails { get; set; }

        public List<ShiftDataResultSocio> Socios { get; set; }
    }

    public class ShiftDataResultEndereco
    {
        public string EnderecoCompleto { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public byte? Ranking { get; set; }
    }

    public class ShiftDataResultTelefone
    {
        public string DDD { get; set; }
        public string Telefone { get; set; }
        public string TipoTelefone { get; set; }
        public string Operadora { get; set; }
        public bool? Procon { get; set; }
        public bool? WhatsApp { get; set; }
        public byte? Ranking { get; set; }
    }

    public class ShiftDataResultEmail
    {
        public string Email { get; set; }
        public bool? Particular { get; set; }
        public byte? Ranking { get; set; }
    }

    public class ShiftDataResultSocio
    { 
        public string CPF { get; set; }
        public string NomeRazao { get; set; }
        public string PercentualParticipacao { get; set; }
        public DateTime? DataEntrada { get; set; }
        public string Cargo { get; set; }
        public string CodigoCbo { get; set; }
        public string DescricaoCbo { get; set; }
    }
}
