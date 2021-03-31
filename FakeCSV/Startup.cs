using FakeCSV.DAL.Context;
using FakeCSV.Data;
using FakeCSV.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FakeCSV
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
          
            services.AddDbContext<FakeCsvDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("FakeCsvDbContext")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<FakeCsvDbContext>();

            services.AddTransient<ISchemaDataService, SchemaDataService>();
            services.AddTransient<IGenerateCsvService, GenerateCsvService>();

            services.AddTransient<DataInitializer>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataInitializer dbInitializer)
        {
          //  dbInitializer.Initialize();

            ////if (env.IsDevelopment())
            ////{
            ////    app.UseDeveloperExceptionPage();
            ////}
            //app.UseDeveloperExceptionPage();

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
