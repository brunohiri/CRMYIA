using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class BannerOperadora
    {
        public long IdBannerOperadora { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdOperadora { get; set; }
        public long? IdBanner { get; set; }

        public virtual Banner IdBannerNavigation { get; set; }
        public virtual Operadora IdOperadoraNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
