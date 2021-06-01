using System;
using System.Collections.Generic;

namespace CRMYIA.Data.Entities
{
    public partial class CalendarioSazonal
    {
        public CalendarioSazonal()
        {
            Visita = new HashSet<Visita>();
        }

        public long IdCalendarioSazonal { get; set; }
        public long? IdCalendario { get; set; }
        public string Descricao { get; set; }
        public string Cor { get; set; }
        public byte? Tipo { get; set; }
        public string GuidId { get; set; }
        public DateTime DataSazonal { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public bool? ExisteCampanha { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public byte? Repete { get; set; }
        public byte? Frequencia { get; set; }

        public virtual Calendario IdCalendarioNavigation { get; set; }
        public virtual ICollection<Visita> Visita { get; set; }
    }
}
