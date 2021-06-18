using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class EnviarUsuarioViewModel
    {
        public string? Descricao { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

    }
}
