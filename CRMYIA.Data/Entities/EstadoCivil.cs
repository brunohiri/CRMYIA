using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class EstadoCivil
    {
        public EstadoCivil()
        {
            Cliente = new HashSet<Cliente>();
        }

        public byte IdEstadoCivil { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
