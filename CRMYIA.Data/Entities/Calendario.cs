using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Calendario
    {
        public Calendario()
        {
            AssinaturaCartao = new HashSet<AssinaturaCartao>();
            CalendarioSazonal = new HashSet<CalendarioSazonal>();
            CampanhaArquivo = new HashSet<CampanhaArquivo>();
            Capa = new HashSet<Capa>();
            Video = new HashSet<Video>();
        }

        public long IdCalendario { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual ICollection<AssinaturaCartao> AssinaturaCartao { get; set; }
        public virtual ICollection<CalendarioSazonal> CalendarioSazonal { get; set; }
        public virtual ICollection<CampanhaArquivo> CampanhaArquivo { get; set; }
        public virtual ICollection<Capa> Capa { get; set; }
        public virtual ICollection<Video> Video { get; set; }
    }
}
