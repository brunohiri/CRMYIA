using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class BannerOperadoraViewModel
    {
        public string IdBanner { get; set; }
        public string IdCampanha { get; set; }
        public long? IdCalendario { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string NomeCampanha { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<BannerOperadora> BannerOperadora { get; set; }
        public virtual Informacao IdInformacaoNavigation { get; set; }
    }
}
