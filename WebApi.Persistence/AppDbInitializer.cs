using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain;
using WebApi.Domain.Models.Helpers;
using WebApi.Domain.ViewModel;

namespace WebApi.Persistence
{
    public static class AppDbInitializer
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Worker))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Worker));

            var registerUsers = new List<RegisterViewModel>
            {
                new()
                {
                    FirstName = "admin",
                    LastName = "user",
                    EmailAddress = "admin.user@email.com",
                    Role = UserRoles.Admin,
                    UserName = "Admin.User"
                },
                new()
                {
                    FirstName = "general",
                    LastName = "user",
                    EmailAddress = "general.user@email.com",
                    Role = UserRoles.Worker,
                    UserName = "General.User"
                }
            };
            foreach (var registerUser in registerUsers)
            {
                var userExist = await userManager.FindByEmailAsync(registerUser.EmailAddress);
                if (userExist == null)
                {
                    var user = new User
                    {
                        FirstName = registerUser.FirstName,
                        LastName = registerUser.LastName,
                        Email = registerUser.EmailAddress,
                        UserName = registerUser.UserName,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    var result = await userManager.CreateAsync(user, registerUser.UserName + "@2022");
                    if (result.Succeeded)
                    {
                        switch (registerUser.Role)
                        {
                            case UserRoles.Admin:
                                await userManager.AddToRoleAsync(user, UserRoles.Admin);
                                break;
                            case UserRoles.Worker:
                                await userManager.AddToRoleAsync(user, UserRoles.Worker);
                                break;
                        }
                    }
                }
            }
        }
    }
}