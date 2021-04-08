using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class CampanhaArquivoViewModel
    {

        public long IdCampanhaArquivo { get; set; }
        public long?IdCampanha { get; set; }
        public string NomeCampanha { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string RedesSociais { get; set; }
        public string TipoPostagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Informacao IdInformacaoNavigation { get; set; }
    }
}
