using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            HistoricoAcesso = new HashSet<HistoricoAcesso>();
            HistoricoProposta = new HashSet<HistoricoProposta>();
            Meta = new HashSet<Meta>();
            NotificacaoIdUsuarioCadastroNavigation = new HashSet<Notificacao>();
            NotificacaoIdUsuarioVisualizarNavigation = new HashSet<Notificacao>();
            PropostaIdUsuarioCorretorNavigation = new HashSet<Proposta>();
            PropostaIdUsuarioNavigation = new HashSet<Proposta>();
            UsuarioCliente = new HashSet<UsuarioCliente>();
            UsuarioHierarquiaIdUsuarioMasterNavigation = new HashSet<UsuarioHierarquia>();
            UsuarioHierarquiaIdUsuarioSlaveNavigation = new HashSet<UsuarioHierarquia>();
            UsuarioPerfil = new HashSet<UsuarioPerfil>();
            Visita = new HashSet<Visita>();
        }

        public long IdUsuario { get; set; }
        public long? IdCorretora { get; set; }
        public byte? IdClassificacao { get; set; }
        public byte? IdProducao { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public DateTime? DataNascimentoAbertura { get; set; }
        public string Codigo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string IP { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Classificacao IdClassificacaoNavigation { get; set; }
        public virtual Corretora IdCorretoraNavigation { get; set; }
        public virtual Producao IdProducaoNavigation { get; set; }
        public virtual ICollection<HistoricoAcesso> HistoricoAcesso { get; set; }
        public virtual ICollection<HistoricoProposta> HistoricoProposta { get; set; }
        public virtual ICollection<Meta> Meta { get; set; }
        public virtual ICollection<Notificacao> NotificacaoIdUsuarioCadastroNavigation { get; set; }
        public virtual ICollection<Notificacao> NotificacaoIdUsuarioVisualizarNavigation { get; set; }
        public virtual ICollection<Proposta> PropostaIdUsuarioCorretorNavigation { get; set; }
        public virtual ICollection<Proposta> PropostaIdUsuarioNavigation { get; set; }
        public virtual ICollection<UsuarioCliente> UsuarioCliente { get; set; }
        public virtual ICollection<UsuarioHierarquia> UsuarioHierarquiaIdUsuarioMasterNavigation { get; set; }
        public virtual ICollection<UsuarioHierarquia> UsuarioHierarquiaIdUsuarioSlaveNavigation { get; set; }
        public virtual ICollection<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual ICollection<Visita> Visita { get; set; }
    }
}
