using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class MotivoDeclinio
    {
        public MotivoDeclinio()
        {
            Proposta = new HashSet<Proposta>();
        }

        public byte IdMotivoDeclinio { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}
