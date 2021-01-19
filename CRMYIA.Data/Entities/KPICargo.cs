using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPICargo
    {
        public KPICargo()
        {
            Meta = new HashSet<Meta>();
        }

        public long IdKPICargo { get; set; }
        public string Cargo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Meta> Meta { get; set; }
    }
}
