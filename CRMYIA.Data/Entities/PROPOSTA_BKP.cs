using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class PROPOSTA_BKP
    {
        public long IdProposta { get; set; }
        public byte? IdModalidade { get; set; }
        public byte? IdPorte { get; set; }
        public long? IdCategoria { get; set; }
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
        public string NumeroProposta { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}
