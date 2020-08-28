using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Visita
    {
        public long IdVisita { get; set; }
        public long? IdProposta { get; set; }
        public byte? IdStatusVisita { get; set; }
        public DateTime? DataAgendamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataVisitaRealizada { get; set; }
        public string Observacao { get; set; }

        public virtual Proposta IdPropostaNavigation { get; set; }
        public virtual StatusVisita IdStatusVisitaNavigation { get; set; }
    }
}
