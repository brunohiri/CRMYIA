using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class VideoViewModel
    {
        public string IdVideo { get; set; }
        public string IdentificadorVideo { get; set; }
        public long IdCampanha { get; set; }
        public long? IdCalendario { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeVideo { get; set; }
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}
