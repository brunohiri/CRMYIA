using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Operadora
    {
        public Operadora()
        {
            OperadoraDocumento = new HashSet<OperadoraDocumento>();
            Produto = new HashSet<Produto>();
        }

        public long IdOperadora { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<OperadoraDocumento> OperadoraDocumento { get; set; }
        public virtual ICollection<Produto> Produto { get; set; }
    }
}
