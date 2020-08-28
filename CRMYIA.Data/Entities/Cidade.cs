﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Cidade
    {
        public Cidade()
        {
            Cliente = new HashSet<Cliente>();
            Corretora = new HashSet<Corretora>();
        }

        public byte IdCidade { get; set; }
        public byte? IdEstado { get; set; }
        public string Descricao { get; set; }
        public string CodigoIBGE { get; set; }
        public bool Ativo { get; set; }

        public virtual Estado IdEstadoNavigation { get; set; }
        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<Corretora> Corretora { get; set; }
    }
}