using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class OperadoraDocumento
    {
        public byte IdOperadoraDocumento { get; set; }
        public long? IdOperadora { get; set; }
        public byte? IdTipoDocumento { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Ativo { get; set; }

        public virtual Operadora IdOperadoraNavigation { get; set; }
        public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; }
    }
}
