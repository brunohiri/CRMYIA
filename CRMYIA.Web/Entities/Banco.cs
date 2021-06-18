using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Banco
    {
        public Banco()
        {
            Proposta = new HashSet<Proposta>();
        }

        public long IdBanco { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}
