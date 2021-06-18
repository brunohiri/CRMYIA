using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class ArquivoLead
    {
        public long IdArquivoLead { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivoOriginal { get; set; }
        public string NomeArquivoTratado { get; set; }
        public int? QtdRegistros { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
