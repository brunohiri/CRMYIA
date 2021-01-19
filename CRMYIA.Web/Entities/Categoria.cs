using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Categoria
    {
        public Categoria()
        {
            Proposta = new HashSet<Proposta>();
        }

        public long IdCategoria { get; set; }
        public long? IdLinha { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Linha IdLinhaNavigation { get; set; }
        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}
