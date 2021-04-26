using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Serialization;
using CRMYIA.Web.Controller;
using CRMYIA.Data.Entities;
using CRMYIA.Business.Util;

namespace CRMYIA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
            .AddRazorRuntimeCompilation();

            //services.AddSignalR();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 102400000;
                //options.KeepAliveInterval = TimeSpan.FromSeconds(3);
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });


            //Configurações para execução no IIS
            services.Configure<IISOptions>(options =>
            {
                //options.ForwardClientCertificate = false;
            });


            //Início - Autentição via cookies
            services.AddAuthentication("CookieAuthentication")
               .AddCookie("CookieAuthentication", config =>
               {
                   config.Cookie.Name = "UserLoginCookie";
                   config.LoginPath = "/Login";
                   config.AccessDeniedPath = "/AccessDenied";
               });
            services.AddControllersWithViews();
            //Fim - Autentição via cookies

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc()
           .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(opt =>
            {
                opt.Cookie.IsEssential = true;
                opt.IdleTimeout = TimeSpan.FromHours(6);
            });

            //Carregar IP corretamente
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });

            //Habilitando CORS
            services.AddCors();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseForwardedHeaders();
            app.UseSession();
            app.UseResponseCompression();

            //Habilitando CORS
            app.UseCors(option => option.AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificacaoController>("/notificacaohub");
            });

            //Set CultureInfo
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



            ////Criação do Calendário Sazonal 
            ////Aténcão verificAr qual usuario será adicionado no IdUsuario referente a Classe Visita por padrão sera o IdUsuario => 2720

            ////List<Visita> ListVisita = new List<Visita>();
            ////int QuantidadeAnos = 2;
            ////List<CalendarioSazonal> ListCalendarioSazonal1 = Util.CalcularFeriado(DateTime.Now.Year, QuantidadeAnos);
            ////List<CalendarioSazonal> ListCalendarioSazonal2 = null;

            ////Business.CalendarioSazonalModel.AddList(ListCalendarioSazonal1);

            ////ListCalendarioSazonal2 = Business.CalendarioSazonalModel.GetList();

            ////foreach (CalendarioSazonal Item in ListCalendarioSazonal2)
            ////{
            ////    ListVisita.Add(new Visita()
            ////    {
            ////        IdStatusVisita = (byte)6,
            ////        Visivel = (byte)1,
            ////        Cor = Item.Cor,
            ////        IdUsuario = 2720,
            ////        IdCalendarioSazonal = Item.IdCalendarioSazonal,
            ////        Descricao = Item.Descricao,
            ////        DataAgendamento = Item.DataSazonal,
            ////        DataInicio = Item.DataInicio,
            ////        DataFim = Item.DataFim,
            ////        DataCadastro = DateTime.Now
            ////    });
            ////}

            ////Business.VisitaModel.AddList(ListVisita);
        }
    }
}
