using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Meta
    {
        public Meta()
        {
            KPIMetaValor = new HashSet<KPIMetaValor>();
            KPIMetaVida = new HashSet<KPIMetaVida>();
        }

        public long IdMeta { get; set; }
        public long? IdUsuario { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
        public DateTime DataMinima { get; set; }
        public DateTime? DataMaxima { get; set; }
        public bool Ativo { get; set; }
        public long? IdKPIMetaVida { get; set; }
        public long? IdKPICargo { get; set; }
        public long? IdKPIServico { get; set; }

        public virtual KPICargo IdKPICargoNavigation { get; set; }
        public virtual KPIServico IdKPIServicoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<KPIMetaValor> KPIMetaValor { get; set; }
        public virtual ICollection<KPIMetaVida> KPIMetaVida { get; set; }
    }
}
