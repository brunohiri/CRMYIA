using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class GrupoCorretorCampanha
    {
        public long IdGrupoCorretorCampanha { get; set; }
        public byte? IdGrupoCorretor { get; set; }
        public long? IdCampanha { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual GrupoCorretor IdGrupoCorretorNavigation { get; set; }
    }
}
