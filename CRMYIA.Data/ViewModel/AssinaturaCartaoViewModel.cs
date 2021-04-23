using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class AssinaturaCartaoViewModel
    {
        public string IdAssinaturaCartao { get; set; }
        public string IdCampanha { get; set; }
        public string Titulo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
