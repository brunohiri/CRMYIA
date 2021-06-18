using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class FaixaEtaria
    {
        public FaixaEtaria()
        {
            PropostaFaixaEtaria = new HashSet<PropostaFaixaEtaria>();
        }

        public byte IdFaixaEtaria { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<PropostaFaixaEtaria> PropostaFaixaEtaria { get; set; }
    }
}
