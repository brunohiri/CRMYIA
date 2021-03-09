using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class MaterialDivulgacaoViewModel
    {
        public string IdCampanhaArquivo { get; set; }
        public string? IdCampanha { get; set; }
        public string Descricao { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Observacao { get; set; }
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
    }
}
