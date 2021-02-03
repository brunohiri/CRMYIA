using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIGrupo
    {
        public KPIGrupo()
        {
            KPIGrupoUsuario = new HashSet<KPIGrupoUsuario>();
        }

        public long IdKPIGrupo { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }

        public virtual ICollection<KPIGrupoUsuario> KPIGrupoUsuario { get; set; }
    }
}
