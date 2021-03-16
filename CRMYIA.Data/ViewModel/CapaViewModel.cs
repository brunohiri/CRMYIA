using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class CapaViewModel
    {
        public string IdCapa { get; set; }
        public string Titulo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Width { get; set; }
        public string Heighgt { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Capa IdCapaNavigation { get; set; }
        public virtual RedeSocial IdRedeSocialNavigation { get; set; }
    }
}
