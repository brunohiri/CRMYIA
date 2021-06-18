using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class GrupoCorretor
    {
        public GrupoCorretor()
        {
            GrupoCorretorCampanha = new HashSet<GrupoCorretorCampanha>();
            GrupoCorretorOperadora = new HashSet<GrupoCorretorOperadora>();
            Usuario = new HashSet<Usuario>();
        }

        public byte IdGrupoCorretor { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<GrupoCorretorCampanha> GrupoCorretorCampanha { get; set; }
        public virtual ICollection<GrupoCorretorOperadora> GrupoCorretorOperadora { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
