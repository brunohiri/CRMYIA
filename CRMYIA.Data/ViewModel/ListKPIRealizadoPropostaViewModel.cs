using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class ListKPIRealizadoPropostaViewModel
    {
        public long IdUsuario { get; set; }
        public decimal? ValorPrevisto { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public int? QuantidadeVidas { get; set; }
    }
}
