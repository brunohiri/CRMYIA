using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class Visita
    {
        public Visita()
        {
            VisitaCampanha = new HashSet<VisitaCampanha>();
        }

        public long IdVisita { get; set; }
        public long? IdProposta { get; set; }
        public byte? IdStatusVisita { get; set; }
        public long? IdUsuario { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataAgendamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataVisitaRealizada { get; set; }
        public string Observacao { get; set; }
        public long? IdCalendarioSazonal { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public byte? Visivel { get; set; }
        public string Cor { get; set; }
        public byte? Tipo { get; set; }
        public string GuidId { get; set; }
        public byte? Repete { get; set; }
        public byte? Frequencia { get; set; }
        public int? Repetir { get; set; }
        public int? Termina { get; set; }
        public string Semana { get; set; }
        public int? MesDataColocacao { get; set; }
        public string MesDiaDaSemana { get; set; }
        public int? MesDia { get; set; }
        public int? SelectMensalmente { get; set; }
        public DateTime? DataTerminaEm { get; set; }

        public virtual CalendarioSazonal IdCalendarioSazonalNavigation { get; set; }
        public virtual Proposta IdPropostaNavigation { get; set; }
        public virtual StatusVisita IdStatusVisitaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<VisitaCampanha> VisitaCampanha { get; set; }
    }
}
