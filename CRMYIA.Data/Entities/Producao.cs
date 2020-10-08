using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Producao
    {
        public Producao()
        {
            Usuario = new HashSet<Usuario>();
        }

        public byte IdProducao { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
