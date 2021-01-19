using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class KPIServico
    {
        public KPIServico()
        {
            Meta = new HashSet<Meta>();
        }

        public long IdKPIServico { get; set; }
        public string Perfil { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Meta> Meta { get; set; }
    }
}
