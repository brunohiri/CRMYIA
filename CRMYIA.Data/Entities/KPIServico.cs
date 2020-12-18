using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIServico
    {
        public KPIServico()
        {
            TipoLead = new HashSet<TipoLead>();
        }

        public long IdKPIServico { get; set; }
        public string Perfil { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<TipoLead> TipoLead { get; set; }
    }
}
