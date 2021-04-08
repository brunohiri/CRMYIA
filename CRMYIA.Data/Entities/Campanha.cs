using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Campanha
    {
        public Campanha()
        {
            AssinaturaCartao = new HashSet<AssinaturaCartao>();
            Banner = new HashSet<Banner>();
            CampanhaArquivo = new HashSet<CampanhaArquivo>();
            CapaRedeSocial = new HashSet<CapaRedeSocial>();
            GrupoCorretorCampanha = new HashSet<GrupoCorretorCampanha>();
            Video = new HashSet<Video>();
        }

        public long IdCampanha { get; set; }
        public long? IdUsuario { get; set; }
        public string Descricao { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public long? QuantidadeDownload { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<AssinaturaCartao> AssinaturaCartao { get; set; }
        public virtual ICollection<Banner> Banner { get; set; }
        public virtual ICollection<CampanhaArquivo> CampanhaArquivo { get; set; }
        public virtual ICollection<CapaRedeSocial> CapaRedeSocial { get; set; }
        public virtual ICollection<GrupoCorretorCampanha> GrupoCorretorCampanha { get; set; }
        public virtual ICollection<Video> Video { get; set; }
    }
}
