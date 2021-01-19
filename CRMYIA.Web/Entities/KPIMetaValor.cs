using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class KPIMetaValor
    {
        public long IdKPIMetaValor { get; set; }
        public string Descricao { get; set; }
        public decimal? Meta { get; set; }
        public byte? Mes { get; set; }
        public int? Ano { get; set; }
        public bool Ativo { get; set; }
        public decimal? Estipulado { get; set; }
        public long? IdMeta { get; set; }

        public virtual Meta IdMetaNavigation { get; set; }
    }
}
