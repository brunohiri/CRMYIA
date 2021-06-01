using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class EnviarCapaRedeSocialViewModel
    {

        public long IdCapa { get; set; }
        public long IdRedeSocial { get; set; }
        public long IdCampanha { get; set; }
        public long? IdCalendario { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    }
}
