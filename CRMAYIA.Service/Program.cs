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
#if DEBUG
            string path = Directory.GetCurrentDirectory();
#endif
#if !DEBUG
            string path = @"C:\CRMYIAService\";
#endif

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
    }
}
