using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class UsuarioHierarquia
    {
        public long IdUsuarioHierarquia { get; set; }
        public long? IdUsuarioMaster { get; set; }
        public long? IdUsuarioSlave { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioMasterNavigation { get; set; }
        public virtual Usuario IdUsuarioSlaveNavigation { get; set; }
    }
}
