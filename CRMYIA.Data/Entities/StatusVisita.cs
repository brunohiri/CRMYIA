using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class StatusVisita
    {
        public StatusVisita()
        {
            Visita = new HashSet<Visita>();
        }

        public byte IdStatusVisita { get; set; }
        public string Descricao { get; set; }
        public string CorHexa { get; set; }
        public string CssClass { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Visita> Visita { get; set; }
    }
}
