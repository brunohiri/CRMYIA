using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Proposta
    {
        public Proposta()
        {
            Documento = new HashSet<Documento>();
            HistoricoProposta = new HashSet<HistoricoProposta>();
            PropostaFaixaEtaria = new HashSet<PropostaFaixaEtaria>();
            Visita = new HashSet<Visita>();
        }

        public long IdProposta { get; set; }
        public byte? IdModalidade { get; set; }
        public long? IdProduto { get; set; }
        public long? IdCliente { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdUsuarioCorretor { get; set; }
        public byte? IdStatusProposta { get; set; }
        public byte? IdMotivoDeclinio { get; set; }
        public byte? IdFaseProposta { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? ProximoContatoComCliente { get; set; }
        public TimeSpan? HorarioParaLigar { get; set; }
        public string PeriodoParaLigar { get; set; }
        public decimal? ValorPrevisto { get; set; }
        public int? QuantidadeVidas { get; set; }
        public bool PossuiPlano { get; set; }
        public string PlanoJaUtilizado { get; set; }
        public string TempoPlano { get; set; }
        public string PreferenciaHospitalar { get; set; }
        public string Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual FaseProposta IdFasePropostaNavigation { get; set; }
        public virtual Modalidade IdModalidadeNavigation { get; set; }
        public virtual MotivoDeclinio IdMotivoDeclinioNavigation { get; set; }
        public virtual Produto IdProdutoNavigation { get; set; }
        public virtual StatusProposta IdStatusPropostaNavigation { get; set; }
        public virtual Usuario IdUsuarioCorretorNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Documento> Documento { get; set; }
        public virtual ICollection<HistoricoProposta> HistoricoProposta { get; set; }
        public virtual ICollection<PropostaFaixaEtaria> PropostaFaixaEtaria { get; set; }
        public virtual ICollection<Visita> Visita { get; set; }
    }
}
