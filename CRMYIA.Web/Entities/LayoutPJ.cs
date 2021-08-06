using System;
using System.Collections.Generic;

namespace CRMYIA.Web.Entities
{
    public partial class LayoutPJ
    {
        public long IdLayoutPJ { get; set; }
        public long? IdFilaItem { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CNAE { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Telefone3 { get; set; }
        public string Telefone4 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string SocioNome { get; set; }
        public string SocioDataNascimento { get; set; }
        public string SocioNomeMae { get; set; }
        public string SocioEndereco { get; set; }
        public string SocioCEP { get; set; }
        public string SocioCidade { get; set; }
        public string SocioUF { get; set; }
        public string SocioTelefone1 { get; set; }
        public string SocioTelefone2 { get; set; }
        public string SocioTelefone3 { get; set; }
        public string SocioTelefone4 { get; set; }
        public string SocioEmail1 { get; set; }
        public string SocioEmail2 { get; set; }
        public string SocioPerfilConsumo { get; set; }

        public virtual FilaItem IdFilaItemNavigation { get; set; }
    }
}
