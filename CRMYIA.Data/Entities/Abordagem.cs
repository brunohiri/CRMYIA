using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Abordagem
    {
        public long IdAbordagem { get; set; }
        public byte? IdAbordagemCategoria { get; set; }
        public long? IdUsuario { get; set; }
        public string Descricao { get; set; }
        public bool Pergunta { get; set; }
        public byte? Ordem { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual AbordagemCategoria IdAbordagemCategoriaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
