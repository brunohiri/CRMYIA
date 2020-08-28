﻿using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Cliente
    {
        public Cliente()
        {
            Proposta = new HashSet<Proposta>();
            UsuarioCliente = new HashSet<UsuarioCliente>();
        }

        public long IdCliente { get; set; }
        public byte? IdCidade { get; set; }
        public long? IdTelefone { get; set; }
        public long? IdEmail { get; set; }
        public byte? IdEstadoCivil { get; set; }
        public byte? IdGenero { get; set; }
        public byte? IdOrigem { get; set; }
        public byte? IdTipoLead { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string CartaoSus { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Cidade IdCidadeNavigation { get; set; }
        public virtual Email IdEmailNavigation { get; set; }
        public virtual EstadoCivil IdEstadoCivilNavigation { get; set; }
        public virtual Genero IdGeneroNavigation { get; set; }
        public virtual Origem IdOrigemNavigation { get; set; }
        public virtual Telefone IdTelefoneNavigation { get; set; }
        public virtual TipoLead IdTipoLeadNavigation { get; set; }
        public virtual ICollection<Proposta> Proposta { get; set; }
        public virtual ICollection<UsuarioCliente> UsuarioCliente { get; set; }
    }
}
