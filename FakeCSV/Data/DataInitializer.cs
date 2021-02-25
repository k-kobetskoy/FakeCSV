using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FakeCSV.Data
{
    public class DataInitializer
    {
        private readonly IConfiguration configuration;
        private readonly FakeCsvDbContext context;
        private readonly ILogger<DataInitializer> logger;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataInitializer(

            IConfiguration configuration,
            FakeCsvDbContext context,
            ILogger<DataInitializer> logger,
            RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.context = context;
            this.logger = logger;
            this.roleManager = roleManager;
        }

        public void Initialize()
        {
            var database = context.Database;

            logger.LogInformation("begin db initialization");

            if (database.GetPendingMigrations().Any())
            {
                logger.LogInformation("there are pending migrations");
                database.Migrate();
                logger.LogInformation("all migrations complete");
            }

            logger.LogInformation("there are no pending migrations");

            try
            {
                InitializeRoles().Wait();
            }
            catch (Exception e)
            {
                logger.LogError(e, "error during roles initialization");
                throw;
            }

        }

        private async Task InitializeRoles()
        {

            if (roleManager.Roles.Any())
            {
                logger.LogInformation("roles initialization not needed");
                return;
            }

            logger.LogInformation("begin roles initialization");


            string[] roles = {
                configuration.GetSection("Roles")["Administrators"],
                configuration.GetSection("Roles")["Users"] };

            foreach (var role in roles)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                {
                    logger.LogError("error while adding role {0}:", role);
                    foreach (var error in result.Errors)
                    {
                        logger.LogError("{0}: {1}",error.Code, error.Description);
                    }
                }
            }
            logger.LogInformation("roles initialization completed sucsessfully");
        }
    }
}
