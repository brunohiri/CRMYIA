using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Banner
    {
        public long IdBanner { get; set; }
        public long? IdInformacao { get; set; }
        public long? IdCampanha { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCalendario { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Informacao IdInformacaoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
