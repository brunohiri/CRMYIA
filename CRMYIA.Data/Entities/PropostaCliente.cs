using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class PropostaCliente
    {
        public long IdPropostaCliente { get; set; }
        public long IdProposta { get; set; }
        public long IdCliente { get; set; }
        public bool Dependente { get; set; }
        public bool? CompraCarencia { get; set; }
        public DateTime? VigenciaContrato { get; set; }
        public string Nome_do_Plano { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Proposta IdPropostaNavigation { get; set; }
    }
}
