using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Documento = new HashSet<Documento>();
            OperadoraDocumento = new HashSet<OperadoraDocumento>();
        }

        public byte IdTipoDocumento { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Documento> Documento { get; set; }
        public virtual ICollection<OperadoraDocumento> OperadoraDocumento { get; set; }
    }
}
