using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Perfil
    {
        public Perfil()
        {
            PerfilModulo = new HashSet<PerfilModulo>();
            UsuarioPerfil = new HashSet<UsuarioPerfil>();
        }

        public byte IdPerfil { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<PerfilModulo> PerfilModulo { get; set; }
        public virtual ICollection<UsuarioPerfil> UsuarioPerfil { get; set; }
    }
}
