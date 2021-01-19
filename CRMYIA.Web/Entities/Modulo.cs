using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Modulo
    {
        public Modulo()
        {
            InverseIdModuloReferenciaNavigation = new HashSet<Modulo>();
            PerfilModulo = new HashSet<PerfilModulo>();
        }

        public long IdModulo { get; set; }
        public long? IdModuloReferencia { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
        public string ToolTip { get; set; }
        public byte? Ordem { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Modulo IdModuloReferenciaNavigation { get; set; }
        public virtual ICollection<Modulo> InverseIdModuloReferenciaNavigation { get; set; }
        public virtual ICollection<PerfilModulo> PerfilModulo { get; set; }
    }
}
