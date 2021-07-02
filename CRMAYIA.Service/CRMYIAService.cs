using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CRMYIA.Data;
using CRMYIA.Business;
using CRMYIA.Data.Entities;
using CRMYIA.Business.YNDICA;
using CRMYIA.Business.Util;

namespace CRMYIA.Service
{
    internal class CRMYIAService : ServiceBase
    {
        private System.Timers.Timer timer;
        private IConfiguration configuration;

        public CRMYIAService(IConfiguration _configuration)
        {
            ServiceName = "CRMYIAService";
            configuration = _configuration;
        }

        private void InitializeTimer()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.AutoReset = true;
                timer.Interval = 60000; //1 minuto
                //timer.Interval = 600000; //10 minutos
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }
        }

        protected override void OnStart(string[] args)
        {
            InitializeTimer();
            timer.Start();
        }

        protected override void OnStop()
        {

        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();


            try
            {
                Console.WriteLine("Iniciando Serviço! " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                try
                {
                    ExecutarServico();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("Finalizando Serviço! " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
            }
            finally
            {
                timer.Enabled = true;
                timer.Start();
            }
        }

        #region Serviço
        public void ExecutarServico()
        {

            try
            {
                #region Verificar Fila para Processamento do Enriquecimento do YNDICA
                List<Fila> ListEntityFila = null;

                ListEntityFila = FilaModel.GetListByStatusFila((short)(EnumeradorModel.StatusFila.Aguardando));

                if ((ListEntityFila !=null) && (ListEntityFila.Count() > 0))
                {
                    ListEntityFila.ToList().ForEach(delegate (Fila ItemFila)
                    {
                        FilaModel.ProcessarFila(ItemFila);
                    });
                }

                #endregion
            }
            catch (Exception)
            {
            }

        }
        #endregion

    }
}
