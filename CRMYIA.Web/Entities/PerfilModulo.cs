using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class PerfilModulo
    {
        public long IdPerfilModulo { get; set; }
        public byte? IdPerfil { get; set; }
        public long? IdModulo { get; set; }
        public bool Ativo { get; set; }

        public virtual Modulo IdModuloNavigation { get; set; }
        public virtual Perfil IdPerfilNavigation { get; set; }
    }
}
