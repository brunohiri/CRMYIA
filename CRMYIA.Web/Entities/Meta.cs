using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Meta
    {
        public long IdMeta { get; set; }
        public long? IdUsuario { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
        public DateTime DataMinima { get; set; }
        public DateTime? DataMaxima { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
