using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Banner
    {
        public long IdBanner { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public long? IdInformacao { get; set; }

        public virtual Informacao IdInformacaoNavigation { get; set; }
    }
}
