using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class CapaRedeSocial
    {
        public long IdCapaRedeSocial { get; set; }
        public long? IdRedeSocial { get; set; }
        public long? IdCapa { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCampanha { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Capa IdCapaNavigation { get; set; }
        public virtual RedeSocial IdRedeSocialNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
