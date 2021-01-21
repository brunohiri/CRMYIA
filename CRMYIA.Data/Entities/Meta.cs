﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
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
        public DateTime DataMinima { get; set; }
        public DateTime? DataMaxima { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<KPIMetaValor> KPIMetaValor { get; set; }
        public virtual ICollection<KPIMetaVida> KPIMetaVida { get; set; }
    }
}
