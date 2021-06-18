using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Email
    {
        public long IdEmail { get; set; }
        public long? IdCliente { get; set; }
        public string EmailConta { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
    }
}
