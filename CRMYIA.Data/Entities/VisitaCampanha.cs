using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class VisitaCampanha
    {
        public long IdVisitaCampanha { get; set; }
        public long? IdVisita { get; set; }
        public long? IdCampanha { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Visita IdVisitaNavigation { get; set; }
    }
}
