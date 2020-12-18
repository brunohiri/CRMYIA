using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPICargo
    {
        public KPICargo()
        {
            TipoLead = new HashSet<TipoLead>();
        }

        public long IdKPICargo { get; set; }
        public string Cargo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<TipoLead> TipoLead { get; set; }
    }
}
