
using AuthenticationService.Infrastructure.Persistence.Context;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Infrastructure.Identity.DataSeeding
{
    internal class IdentityDbInitializer(IRoleService roleService, ApplicationDbContext context,IAuthenticationService authenticationService,
       
      ILogger<IIdentityDbInitializer> logger) : IIdentityDbInitializer
    {
        public async Task InitilaizeAssync()
        {



            try
            {


                if (context.Database.GetPendingMigrations().Any())
                    await context.Database.MigrateAsync();


                if (!await context.Roles.AnyAsync())
                {
                    var roles = new List<Role>
                {
                    new Role("Admin"),
                    new Role("User")
                };
                    foreach (var role in roles)
                    {
                        await context.Roles.AddAsync(role);
                    }
                }


                if (!await context.Users.AnyAsync())
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "Admin@Gamil.com"

                    };


                    var result = await authenticationService.RegisterAsync(adminUser.Email, "Admin#123", "admin", "Admin");



                    if (result.IsSuccess == false)
                    {
                    foreach (var row in result.ValidationErrors)
                    {

                        foreach(var error in row.Value)
                        {

                        logger.LogError("Error creating admin user: {Error}", error.Message);
                 
                        }
                    }



                    }


                    await roleService.AssignRoleAsync(result.Value, "Admin");


                }



            }




            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the identity database.");
                throw;
            }



        }

    }
}
