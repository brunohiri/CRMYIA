using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class TipoLead
    {
        public TipoLead()
        {
            Cliente = new HashSet<Cliente>();
            KPIMetaValor = new HashSet<KPIMetaValor>();
            KPIMetaVida = new HashSet<KPIMetaVida>();
        }

        public byte IdTipoLead { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime? MetaDe { get; set; }
        public DateTime? MetaAte { get; set; }
        public long? IdKPIServico { get; set; }
        public long? IdKPICargo { get; set; }

        public virtual KPICargo IdKPICargoNavigation { get; set; }
        public virtual KPIServico IdKPIServicoNavigation { get; set; }
        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<KPIMetaValor> KPIMetaValor { get; set; }
        public virtual ICollection<KPIMetaVida> KPIMetaVida { get; set; }
    }
}
