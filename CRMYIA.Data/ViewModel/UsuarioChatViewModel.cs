using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class UsuarioChatViewModel
    {
        public long IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public string DataMensagem { get; set; }
        public string Mensagem { get; set; }
        public string Imagem { get; set; }
    }
}
