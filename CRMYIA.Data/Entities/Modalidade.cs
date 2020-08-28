﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Modalidade
    {
        public Modalidade()
        {
            Proposta = new HashSet<Proposta>();
        }

        public byte IdModalidade { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}