﻿using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class ListaClienteViewModel
    {
        public long IdCliente { get; set; }
        public string IdClienteString { get; set; }
        public string Nome { get; set; }
        public string OrigemDescricao { get; set; }
        public string TipoLeadDescricao { get; set; }
        public string CorretorNome { get; set; }
        public string CidadeNome { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public int QtdTelefone { get; set; }
        public int QtdEmail { get; set; }
    }
}
