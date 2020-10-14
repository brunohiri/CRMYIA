using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIMeta
    {
        public long IdKPIMeta { get; set; }
        public byte? IdTipoLead { get; set; }
        public decimal? Meta { get; set; }
        public byte? Mes { get; set; }
        public byte? Ano { get; set; }
        public bool Ativo { get; set; }

        public virtual TipoLead IdTipoLeadNavigation { get; set; }
    }
}
