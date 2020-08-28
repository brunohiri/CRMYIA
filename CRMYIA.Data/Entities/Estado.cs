using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Estado
    {
        public Estado()
        {
            Cidade = new HashSet<Cidade>();
        }

        public byte IdEstado { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Cidade> Cidade { get; set; }
    }
}
