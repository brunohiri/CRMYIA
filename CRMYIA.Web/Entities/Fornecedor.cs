using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Fornecedor
    {
        public Fornecedor()
        {
            Fila = new HashSet<Fila>();
            FornecedorConsulta = new HashSet<FornecedorConsulta>();
        }

        public byte IdFornecedor { get; set; }
        public string Descricao { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string TokenAPI { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Fila> Fila { get; set; }
        public virtual ICollection<FornecedorConsulta> FornecedorConsulta { get; set; }
    }
}
