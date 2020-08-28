using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class StatusProposta
    {
        public StatusProposta()
        {
            Proposta = new HashSet<Proposta>();
        }

        public byte IdStatusProposta { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}
