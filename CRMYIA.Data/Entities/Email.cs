using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Email
    {
        public Email()
        {
            Cliente = new HashSet<Cliente>();
        }

        public long IdEmail { get; set; }
        public string EmailConta { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
