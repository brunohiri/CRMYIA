using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Corretora
    {
        public Corretora()
        {
            Usuario = new HashSet<Usuario>();
        }

        public long IdCorretora { get; set; }
        public int? IdCidade { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cidade IdCidadeNavigation { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
