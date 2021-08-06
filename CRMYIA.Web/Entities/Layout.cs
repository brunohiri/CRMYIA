using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Layout
    {
        public Layout()
        {
            Fila = new HashSet<Fila>();
        }

        public byte IdLayout { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Fila> Fila { get; set; }
    }
}
