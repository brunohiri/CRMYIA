using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class StatusLead
    {
        public byte IdStatusLead { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
