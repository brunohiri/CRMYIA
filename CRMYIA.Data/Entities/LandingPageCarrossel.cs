using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class LandingPageCarrossel
    {
        public long IdLandingPageCarrossel { get; set; }
        public long? IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
