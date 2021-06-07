using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIMetaIndividual
    {
        public KPIMetaIndividual()
        {
            KPIMetaValorIndividual = new HashSet<KPIMetaValorIndividual>();
            KPIMetaVidaIndividual = new HashSet<KPIMetaVidaIndividual>();
        }

        public long IdMetaIndividual { get; set; }
        public long? IdKPIGrupoUsuario { get; set; }
        public DateTime DataMinima { get; set; }
        public DateTime DataMaxima { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIGrupoUsuario IdKPIGrupoUsuarioNavigation { get; set; }
        public virtual ICollection<KPIMetaValorIndividual> KPIMetaValorIndividual { get; set; }
        public virtual ICollection<KPIMetaVidaIndividual> KPIMetaVidaIndividual { get; set; }
    }
}
