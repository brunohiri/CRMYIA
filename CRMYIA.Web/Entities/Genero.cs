using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Genero
    {
        public Genero()
        {
            Cliente = new HashSet<Cliente>();
        }

        public byte IdGenero { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
