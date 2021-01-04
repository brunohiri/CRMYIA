using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Origem
    {
        public Origem()
        {
            Cliente = new HashSet<Cliente>();
        }

        public byte IdOrigem { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
