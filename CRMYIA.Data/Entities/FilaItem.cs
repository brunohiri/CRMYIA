using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class FilaItem
    {
        public FilaItem()
        {
            LayoutPJ = new HashSet<LayoutPJ>();
        }

        public long IdFilaItem { get; set; }
        public long? IdFila { get; set; }
        public long? IdFornecedorConsulta { get; set; }
        public string Documento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Processado { get; set; }

        public virtual Fila IdFilaNavigation { get; set; }
        public virtual FornecedorConsulta IdFornecedorConsultaNavigation { get; set; }
        public virtual ICollection<LayoutPJ> LayoutPJ { get; set; }
    }
}
