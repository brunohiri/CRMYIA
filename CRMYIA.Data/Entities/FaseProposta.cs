using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class FaseProposta
    {
        public FaseProposta()
        {
            Proposta = new HashSet<Proposta>();
        }

        public byte IdFaseProposta { get; set; }
        public string Descricao { get; set; }
        public string DescricaoDetalhada { get; set; }
        public int? TempoLimite { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}
