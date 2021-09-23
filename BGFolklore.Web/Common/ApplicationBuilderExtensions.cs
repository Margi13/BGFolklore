using BGFolklore.Common;
using BGFolklore.Common.Common;
using BGFolklore.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Common
{
    public static class ApplicationBuilderExtensions
    {
        private static List<string> roleNames = new List<string>()
        {
            Constants.AdminRoleName,
            Constants.ModeratorRoleName,
            Constants.OrgRoleName,
            Constants.UserRoleName

        };
        private static List<string> userNames = new List<string>()
        {
            "admin",
            "magi"

        };

        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            var serviceScoreFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScoreFactory.CreateScope())
            {
                await AddRolesAsync(scope);
                await AddUsersAsync(scope);
            }
        }

        public static async Task AddRolesAsync(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        public static async Task AddUsersAsync(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string password = "abc123";
            foreach (var userName in userNames)
            {
                if (await userManager.FindByNameAsync(userName) == null)
                {
                    User user = new User();
                    user.UserName = userName;
                    var email = userName + "@admin.bg";
                    user.Email = EncryptDecrypt.Encryption(email);
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, roleNames[0]);
                }
            }
        }
    }
}
