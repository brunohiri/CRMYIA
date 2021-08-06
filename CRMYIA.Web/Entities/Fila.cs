using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Fila
    {
        public Fila()
        {
            FilaItem = new HashSet<FilaItem>();
        }

        public long IdFila { get; set; }
        public byte? IdStatusFila { get; set; }
        public long? IdUsuario { get; set; }
        public byte? IdFornecedor { get; set; }
        public byte? IdLayout { get; set; }
        public string NomeJob { get; set; }
        public string CaminhoArquivoEntrada { get; set; }
        public string NomeArquivoEntrada { get; set; }
        public string CaminhoArquivoSaida { get; set; }
        public string NomeArquivoSaida { get; set; }
        public int? QtdEntrada { get; set; }
        public int? QtdProcessado { get; set; }
        public int? QtdSaida { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public DateTime? DataSaida { get; set; }
        public string IP { get; set; }

        public virtual Fornecedor IdFornecedorNavigation { get; set; }
        public virtual Layout IdLayoutNavigation { get; set; }
        public virtual StatusFila IdStatusFilaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<FilaItem> FilaItem { get; set; }
    }
}
