using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIMeta
    {
        public KPIMeta()
        {
            KPIGrupoUsuario = new HashSet<KPIGrupoUsuario>();
            KPIMetaValor = new HashSet<KPIMetaValor>();
            KPIMetaVida = new HashSet<KPIMetaVida>();
        }

        public long IdMeta { get; set; }
        public long? IdKPIGrupo { get; set; }
        public DateTime DataMinima { get; set; }
        public DateTime DataMaxima { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIGrupo IdKPIGrupoNavigation { get; set; }
        public virtual ICollection<KPIGrupoUsuario> KPIGrupoUsuario { get; set; }
        public virtual ICollection<KPIMetaValor> KPIMetaValor { get; set; }
        public virtual ICollection<KPIMetaVida> KPIMetaVida { get; set; }
    }
}
