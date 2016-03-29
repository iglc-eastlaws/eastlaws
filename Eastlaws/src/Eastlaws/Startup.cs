using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Eastlaws.Services;
using Microsoft.Extensions.Configuration;

namespace Eastlaws
{
    public class Startup
    {
        // Adding Configuration file Instead of web.config ! (Besada Hanna 2016-03-29)
        private IConfiguration m_Config = null;
        public Startup()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("Settings.json"); 
            m_Config = builder.Build();
        }
        public IConfiguration Configuration
        {
            get
            {
                return m_Config;
            }
        }
        


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc();
            services.AddScoped<IRestaurantData, InMemopryRestaurantData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
          
            app.UseIISPlatformHandler();
            app.UseMvcWithDefaultRoute();


            string ConnectionString = Configuration["ConnectionString"];


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!!");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
