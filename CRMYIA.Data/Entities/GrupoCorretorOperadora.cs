using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class GrupoCorretorOperadora
    {
        public long IdGrupoCorretorOperadora { get; set; }
        public long? IdOperadora { get; set; }
        public byte? IdGrupoCorretor { get; set; }

        public virtual GrupoCorretor IdGrupoCorretorNavigation { get; set; }
        public virtual Operadora IdOperadoraNavigation { get; set; }
    }
}
