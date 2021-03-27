using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class BannerOperadoraCanvasViewModel
    {
        public long? IdOperadora { get; set; }
        public long? IdBanner { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<BannerOperadora> BannerOperadora { get; set; }
        public virtual Operadora IdOperadoraNavigation { get; set; }
        public virtual Informacao IdInformacaoNavigation { get; set; }
    }
}
