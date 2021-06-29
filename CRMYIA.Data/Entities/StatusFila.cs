using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class StatusFila
    {
        public StatusFila()
        {
            Fila = new HashSet<Fila>();
        }

        public byte IdStatusFila { get; set; }
        public string Descricao { get; set; }
        public string CssClass { get; set; }
        public string CssIcon { get; set; }
        public string ToolTip { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Fila> Fila { get; set; }
    }
}
