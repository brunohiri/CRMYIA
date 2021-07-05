using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.ServiceProcess;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;
using CRMYIA.Service;

namespace CRMYIA
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            try
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: Program\nInício Serviço\n");
//#if DEBUG
            //string path = Directory.GetCurrentDirectory();
//#endif
//#if !DEBUG
                string path = @"C:\CRMYIAService\";
//#endif

                var builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                using (var service = new CRMYIAService(Configuration))
                {
                    //service.ExecutarServico();
                    ServiceBase.Run(service);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"C:\Temp\LogCRMYIA_Service_{0:dd_MM_yyyy}.txt", DateTime.Now), DateTime.Now.ToString("HH:mm:ss.fff") + "\nMétodo: Program\n" + ex.Message + "\n");
            }
        }
    }
}
