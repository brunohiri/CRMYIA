using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Operadora
    {
        public Operadora()
        {
            GrupoCorretorOperadora = new HashSet<GrupoCorretorOperadora>();
            OperadoraDocumento = new HashSet<OperadoraDocumento>();
            Produto = new HashSet<Produto>();
        }

        public long IdOperadora { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }

        public virtual ICollection<GrupoCorretorOperadora> GrupoCorretorOperadora { get; set; }
        public virtual ICollection<OperadoraDocumento> OperadoraDocumento { get; set; }
        public virtual ICollection<Produto> Produto { get; set; }
    }
}
