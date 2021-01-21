using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class TipoLead
    {
        public TipoLead()
        {
            Cliente = new HashSet<Cliente>();
        }

        public byte IdTipoLead { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public byte? IdTipoLead1 { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
