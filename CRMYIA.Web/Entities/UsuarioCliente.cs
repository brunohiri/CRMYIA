using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class UsuarioCliente
    {
        public long IdUsuarioCliente { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCliente { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
