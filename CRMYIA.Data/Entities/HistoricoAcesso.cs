using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class HistoricoAcesso
    {
        public long IdHistoricoAcesso { get; set; }
        public long? IdUsuario { get; set; }
        public DateTime DataAcesso { get; set; }
        public string IP { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
