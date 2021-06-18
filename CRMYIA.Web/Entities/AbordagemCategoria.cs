using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class AbordagemCategoria
    {
        public AbordagemCategoria()
        {
            Abordagem = new HashSet<Abordagem>();
        }

        public byte IdAbordagemCategoria { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Abordagem> Abordagem { get; set; }
    }
}
