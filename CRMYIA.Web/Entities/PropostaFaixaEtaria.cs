using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class PropostaFaixaEtaria
    {
        public long IdPropostaFaixaEtaria { get; set; }
        public long? IdProposta { get; set; }
        public byte? IdFaixaEtaria { get; set; }
        public int? Quantidade { get; set; }
        public bool Ativo { get; set; }

        public virtual FaixaEtaria IdFaixaEtariaNavigation { get; set; }
        public virtual Proposta IdPropostaNavigation { get; set; }
    }
}
