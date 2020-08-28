using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Telefone
    {
        public Telefone()
        {
            Cliente = new HashSet<Cliente>();
        }

        public long IdTelefone { get; set; }
        public byte? IdOperadoraTelefone { get; set; }
        public string DDD { get; set; }
        public string Telefone1 { get; set; }
        public bool WhatsApp { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual OperadoraTelefone IdOperadoraTelefoneNavigation { get; set; }
        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
