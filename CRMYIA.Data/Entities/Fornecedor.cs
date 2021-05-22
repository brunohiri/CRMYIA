using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Fornecedor
    {
        public Fornecedor()
        {
            FornecedorConsulta = new HashSet<FornecedorConsulta>();
        }

        public byte IdFornecedor { get; set; }
        public string Descricao { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string TokenAPI { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<FornecedorConsulta> FornecedorConsulta { get; set; }
    }
}
