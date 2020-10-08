using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Classificacao
    {
        public Classificacao()
        {
            Usuario = new HashSet<Usuario>();
        }

        public byte IdClassificacao { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
