using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class RedeSocial
    {
        public RedeSocial()
        {
            CapaRedeSocial = new HashSet<CapaRedeSocial>();
        }

        public long IdRedeSocial { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<CapaRedeSocial> CapaRedeSocial { get; set; }
    }
}
