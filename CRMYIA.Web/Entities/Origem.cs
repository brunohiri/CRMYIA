using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Origem
    {
        public byte IdOrigem { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
