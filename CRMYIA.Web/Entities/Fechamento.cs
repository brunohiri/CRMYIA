using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Fechamento
    {
        public DateTime Inclusao { get; set; }
        public string Assistente_Original { get; set; }
        public int Cod_Corretor { get; set; }
        public string Corretor { get; set; }
        public string Apelido { get; set; }
        public string Segurado { get; set; }
        public string Tipo { get; set; }
        public string Operadora { get; set; }
        public string Modalidade { get; set; }
        public double Boleto { get; set; }
        public string Producao_Classificacao { get; set; }
        public string Beneficiario_Producao { get; set; }
        public string Status { get; set; }
        public double Proposta { get; set; }
        public string Grade_utilizada { get; set; }
    }
}
