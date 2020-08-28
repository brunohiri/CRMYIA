using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class TipoLead
    {
        public TipoLead()
        {
            Cliente = new HashSet<Cliente>();
        }

        public byte IdTipoLead { get; set; }
        public string Descricao { get; set; }
        public int? MetaDe { get; set; }
        public int? MetaAte { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
