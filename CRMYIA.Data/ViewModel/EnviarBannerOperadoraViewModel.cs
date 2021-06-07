using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class EnviarBannerOperadoraViewModel
    {
        public string IdBanner { get; set; }
        public long IdInformacao { get; set; }
        public string IdCampanha { get; set; }
        public long? IdCalendario { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public string IdOperadora { get; set; }
    }
}
