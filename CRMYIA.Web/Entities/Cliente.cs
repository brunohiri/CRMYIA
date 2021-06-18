using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Cliente
    {
        public Cliente()
        {
            Email = new HashSet<Email>();
            Proposta = new HashSet<Proposta>();
            PropostaCliente = new HashSet<PropostaCliente>();
            Telefone = new HashSet<Telefone>();
            UsuarioCliente = new HashSet<UsuarioCliente>();
        }

        public long IdCliente { get; set; }
        public long? IdClienteReferencia { get; set; }
        public int? IdCidade { get; set; }
        public byte? IdEstadoCivil { get; set; }
        public byte? IdGenero { get; set; }
        public byte? IdOrigem { get; set; }
        public byte? IdTipoLead { get; set; }
        public long? IdArquivoLead { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string CartaoSus { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }
        public string SituacaoCadastral { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Observacao { get; set; }
        public string OperadoraLead { get; set; }
        public string ProdutoLead { get; set; }
        public bool StatusPlanoLead { get; set; }
        public DateTime? DataAdesaoLead { get; set; }
        public bool IsLead { get; set; }
        public bool Titular { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public bool? StatusNaoIncomodar { get; set; }

        public virtual ArquivoLead IdArquivoLeadNavigation { get; set; }
        public virtual Cidade IdCidadeNavigation { get; set; }
        public virtual EstadoCivil IdEstadoCivilNavigation { get; set; }
        public virtual Genero IdGeneroNavigation { get; set; }
        public virtual Origem IdOrigemNavigation { get; set; }
        public virtual TipoLead IdTipoLeadNavigation { get; set; }
        public virtual ICollection<Email> Email { get; set; }
        public virtual ICollection<Proposta> Proposta { get; set; }
        public virtual ICollection<PropostaCliente> PropostaCliente { get; set; }
        public virtual ICollection<Telefone> Telefone { get; set; }
        public virtual ICollection<UsuarioCliente> UsuarioCliente { get; set; }
    }
}
