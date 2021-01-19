using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Documento
    {
        public long IdDocumento { get; set; }
        public long? IdProposta { get; set; }
        public byte? IdTipoDocumento { get; set; }
        public string Descricao { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Proposta IdPropostaNavigation { get; set; }
        public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; }
    }
}
