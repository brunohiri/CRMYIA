using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Informacao
    {
        public Informacao()
        {
            Banner = new HashSet<Banner>();
            CampanhaArquivo = new HashSet<CampanhaArquivo>();
        }

        public long IdInformacao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string Titulo { get; set; }

        public virtual ICollection<Banner> Banner { get; set; }
        public virtual ICollection<CampanhaArquivo> CampanhaArquivo { get; set; }
    }
}
