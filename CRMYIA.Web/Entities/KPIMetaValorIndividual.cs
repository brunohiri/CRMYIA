﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class KPIMetaValorIndividual
    {
        public long IdKPIMetaValorIndividual { get; set; }
        public long? IdMetaIndividual { get; set; }
        public string Descricao { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
        public byte? Mes { get; set; }
        public int? Ano { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIMetaIndividual IdMetaIndividualNavigation { get; set; }
    }
}
