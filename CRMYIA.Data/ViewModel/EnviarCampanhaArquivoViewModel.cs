using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class EnviarCampanhaArquivoViewModel
    {
        public long IdCampanhaArquivo { get; set; }
        public long IdCampanha { get; set; }
        public long IdCalendario { get; set; }
        public long IdInformacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
    }
}
