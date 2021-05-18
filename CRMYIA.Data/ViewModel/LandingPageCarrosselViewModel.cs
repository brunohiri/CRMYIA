using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class LandingPageCarrosselViewModel
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
