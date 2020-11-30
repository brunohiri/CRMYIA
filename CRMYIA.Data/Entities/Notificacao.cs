using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Notificacao
    {
        public long IdNotificacao { get; set; }
        public long? IdUsuarioCadastro { get; set; }
        public long? IdUsuarioVisualizar { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public bool Visualizado { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioCadastroNavigation { get; set; }
        public virtual Usuario IdUsuarioVisualizarNavigation { get; set; }
    }
}
