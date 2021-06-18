using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class UsuarioPerfil
    {
        public long IdUsuarioPerfil { get; set; }
        public byte? IdPerfil { get; set; }
        public long? IdUsuario { get; set; }
        public bool Ativo { get; set; }

        public virtual Perfil IdPerfilNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
