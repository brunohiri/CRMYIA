using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class NotificacaoMensagem
    {
        public long IdNotificacaoMensagem { get; set; }
        public long? IdUsuarioDe { get; set; }
        public long? IdUsuarioPara { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Visualizado { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioDeNavigation { get; set; }
        public virtual Usuario IdUsuarioParaNavigation { get; set; }
    }
}
