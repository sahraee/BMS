using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BMS.IOC;
using BMS.Persistence.Contexts;
using BMS.Api.MiddleWares;

namespace BMS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.AddDbContext<BMSDBContext>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Book Management System", Version = "v1"});
                var xmlPath = Path.Combine(System.AppContext.BaseDirectory, "BMS.API.xml");
                c.IncludeXmlComments(xmlPath);
                 xmlPath = Path.Combine(System.AppContext.BaseDirectory, "BMS.DTO.xml");
                c.IncludeXmlComments(xmlPath);

            });

            

            //Call Inject My Services
            RegisterMyServices(services);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
          
            app.UseStatusCodePages();           
            app.UseRouting();       
            app.UseTransactionMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
                  
        }

        public static void RegisterMyServices(IServiceCollection services)
        {        
            DependencyContainer.RegisterServices(services);
        }
    }
}
