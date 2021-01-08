using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class NotificacaoMensagemViewModel
    {
        public string De { get; set; }
        public string Para { get; set; }
        public string  Nome { get; set; }
        public string Mensagem { get; set; }
        public string Imagem { get; set; }
        public string DataCadastro { get; set; }
        public DateTime? DataOrdem { get; set; }
    }
}
