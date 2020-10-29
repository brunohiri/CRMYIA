using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Linha
    {
        public Linha()
        {
            Categoria = new HashSet<Categoria>();
        }

        public long IdLinha { get; set; }
        public long? IdProduto { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Produto IdProdutoNavigation { get; set; }
        public virtual ICollection<Categoria> Categoria { get; set; }
    }
}
