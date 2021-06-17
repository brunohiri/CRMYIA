using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class UsuarioViewModel
    {
        public long IdUsuario { get; set; }
        public string Nome { get; set; }
        public string IdUsuarioString { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<UsuarioPerfil> UsuarioPerfil { get; set; }
    }
}
