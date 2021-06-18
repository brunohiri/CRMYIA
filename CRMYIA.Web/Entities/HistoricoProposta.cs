using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class HistoricoProposta
    {
        public long IdHistoricoProposta { get; set; }
        public long? IdProposta { get; set; }
        public long? IdUsuario { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public bool? UsuarioMasterSlave { get; set; }

        public virtual Proposta IdPropostaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
