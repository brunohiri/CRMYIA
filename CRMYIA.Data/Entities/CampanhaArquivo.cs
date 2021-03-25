using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class CampanhaArquivo
    {
        public long IdCampanhaArquivo { get; set; }
        public long? IdCampanha { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string RedesSociais { get; set; }
        public string TipoPostagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public long? IdInformacao { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Informacao IdInformacaoNavigation { get; set; }
    }
}
