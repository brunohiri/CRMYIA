using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class OperadoraTelefone
    {
        public OperadoraTelefone()
        {
            Telefone = new HashSet<Telefone>();
        }

        public byte IdOperadoraTelefone { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Telefone> Telefone { get; set; }
    }
}
