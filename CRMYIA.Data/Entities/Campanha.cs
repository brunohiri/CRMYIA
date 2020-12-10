using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Campanha
    {
        public Campanha()
        {
            CampanhaArquivo = new HashSet<CampanhaArquivo>();
        }

        public long IdCampanha { get; set; }
        public long? IdUsuario { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<CampanhaArquivo> CampanhaArquivo { get; set; }
    }
}
