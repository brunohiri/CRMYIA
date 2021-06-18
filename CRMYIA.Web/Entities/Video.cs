using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class Video
    {
        public long IdVideo { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCampanha { get; set; }
        public long? IdCalendario { get; set; }
        public string IdentificadorVideo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeVideo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Calendario IdCalendarioNavigation { get; set; }
        public virtual Campanha IdCampanhaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
