using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class ClienteViewModel
    {
        public long IdCliente { get; set; }

        public string IdClienteCriptografado { get; set; }

        public string Documento { get; set; }

        public string Nome { get; set; }

        public string Celular { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Endereco { get; set; }

        public string Situacao { get; set; }

        public string DataNascAbertura { get; set; }

        public long? IdOrigem { get; set; }

        public string? NomeCidade { get; set; }

        public bool? StatusPlanoLead { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
