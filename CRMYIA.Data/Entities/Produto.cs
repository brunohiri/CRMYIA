﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Produto
    {
        public Produto()
        {
            Proposta = new HashSet<Proposta>();
        }

        public long IdProduto { get; set; }
        public long? IdOperadora { get; set; }
        public string Descricao { get; set; }
        public string DescricaoDetalhada { get; set; }
        public string RegistroANS { get; set; }
        public string RegistroPlano { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Operadora IdOperadoraNavigation { get; set; }
        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}