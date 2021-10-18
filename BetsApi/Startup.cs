using BetsApi_Business.Interfaces;
using BetsApi_Business.Repository;
using BetsApi_Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetsApi
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

            services.AddCors();

//             services.AddCors((options) =>
//             {
//                 options.AddPolicy(name: "NotFightClubLocal", builder =>
//                 {
//                     builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
//                     .AllowAnyHeader()
//                     .AllowAnyMethod();
//                 });
//             });

            services.AddScoped<IWagerRepo, WagerRepo>();
            //services.AddSingleton<IWagerRepo, WagerRepo>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetsApi", Version = "v1" });
            });
            services.AddDbContext<WageDbContext>(options => {
                if (!options.IsConfigured) {
                    options.UseSqlServer(Configuration.GetConnectionString("Default"));
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetsApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors( options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        //    app.UseCors("NotFightClubLocal");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
