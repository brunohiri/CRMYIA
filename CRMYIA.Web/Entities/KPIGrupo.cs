using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class KPIGrupo
    {
        public KPIGrupo()
        {
            KPIGrupoUsuario = new HashSet<KPIGrupoUsuario>();
            KPIMeta = new HashSet<KPIMeta>();
        }

        public long IdKPIGrupo { get; set; }
        public long? IdUsuario { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<KPIGrupoUsuario> KPIGrupoUsuario { get; set; }
        public virtual ICollection<KPIMeta> KPIMeta { get; set; }
    }
}
