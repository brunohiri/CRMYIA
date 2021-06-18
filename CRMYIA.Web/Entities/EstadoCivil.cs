using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class EstadoCivil
    {
        public byte IdEstadoCivil { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
