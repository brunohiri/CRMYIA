using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class FornecedorConsulta
    {
        public long IdFornecedorConsulta { get; set; }
        public byte? IdFornecedor { get; set; }
        public long? IdUsuario { get; set; }
        public string Documento { get; set; }
        public string Metodo { get; set; }
        public string RetornoJson { get; set; }
        public DateTime DataConsulta { get; set; }
        public string IP { get; set; }

        public virtual Fornecedor IdFornecedorNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
