﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIMetaValor
    {
        public long IdKPIMetaValor { get; set; }
        public long? IdMeta { get; set; }
        public string Descricao { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
        public byte? Mes { get; set; }
        public int? Ano { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIMeta IdMetaNavigation { get; set; }
    }
}