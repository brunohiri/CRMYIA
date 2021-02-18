using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class ListaKPIUsuarioViewModel
    {
        public long IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Corretora { get; set; }
        public string DescricaoPerfil { get; set; }
        public DateTime DataCadastro { get; set; }
        public string CaminhoFoto { get; set; }
        public string NomeFoto { get; set; }
        public bool Ativo { get; set; }
    }
}
