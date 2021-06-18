using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class KPIMetaVidaIndividual
    {
        public long IdKPIMetaVidaIndividual { get; set; }
        public long? IdMetaIndividual { get; set; }
        public string Descricao { get; set; }
        public int? ValorMinimo { get; set; }
        public int? ValorMaximo { get; set; }
        public byte? Mes { get; set; }
        public int? Ano { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIMetaIndividual IdMetaIndividualNavigation { get; set; }
    }
}
