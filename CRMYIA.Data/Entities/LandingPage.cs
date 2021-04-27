using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class LandingPage
    {
        public long IdLandingPage { get; set; }
        public long? IdUsuario { get; set; }
        public bool Individual { get; set; }
        public bool Familiar { get; set; }
        public bool Empresarial { get; set; }
        public int? Vidas { get; set; }
        public bool PossuiPlano { get; set; }
        public bool PossuiCNPJ { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string IP { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Profissao { get; set; }
        public bool PossuiVeiculo { get; set; }
        public bool BuscaVeiculo { get; set; }
        public bool Casa { get; set; }
        public bool Condominio { get; set; }
        public bool Apartamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
