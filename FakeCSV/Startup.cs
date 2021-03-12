using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.DAL.Context;
using FakeCSV.Data;
using FakeCSV.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FakeCSV
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup( IConfiguration configuration)
        {
            this.configuration = configuration;
        }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FakeCsvDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<FakeCsvDbContext>();

            services.AddTransient<ISchemaDataService, SchemaDataService>();
            //services.AddTransient<ICsvGenerateDataService, CsvGenerateDataService>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddTransient<DataInitializer>();
        }

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataInitializer dbInitializer)
        {
            dbInitializer.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();    
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
