using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class EnviarCalendarioSazonalViewModel
    {
        public long IdCalendarioSazonal { get; set; }
        public long IdVisita { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public string Cor { get; set; }
        public byte? Tipo { get; set; }
        public bool ExisteCampanha { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime DataSazonal { get; set; }
        public DateTime DataTerminaEm { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string DataInicioFim { get; set; }
        public bool Ativo { get; set; }


        ////Repetição
        public int Repete { get; set; }
        public int Frequencia { get; set; }
        public int Termina { get; set; }
        public int Repetir { get; set; }
        public string Semana { get; set; }

        //Semanalmente
        public int MesDataColocacao { get; set; }
        public string MesDiaDaSemana { get; set; }
        public int MesDia { get; set; }
        public int SelectMensalmente { get; set; }
    }
}
