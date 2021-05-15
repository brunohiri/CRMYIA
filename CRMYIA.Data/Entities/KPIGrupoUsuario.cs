using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class KPIGrupoUsuario
    {
        public long IdKPIGrupoUsuario { get; set; }
        public long? IdKPIGrupo { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdMeta { get; set; }
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime? Saida { get; set; }
        public string Motivo { get; set; }
        public string CaminhoFoto { get; set; }
        public string NomeFoto { get; set; }
        public bool? Grupo { get; set; }
        public bool Ativo { get; set; }

        public virtual KPIGrupo IdKPIGrupoNavigation { get; set; }
        public virtual KPIMeta IdMetaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
