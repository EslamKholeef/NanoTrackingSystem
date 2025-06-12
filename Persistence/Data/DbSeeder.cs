using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await CreateRolesAsync(roleManager);
            await CreateUsersAsync(userManager);
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task CreateUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var usersToCreate = new List<(ApplicationUser User, string Password, string Role)>
            {
                (new ApplicationUser
                {
                    UserName = "admin@nanohealth.com",
                    Email = "admin@nanohealth.com",
                    FirstName = "Ahmed",
                    LastName = "Al-Rashid",
                    Department = "IT",
                    JobTitle = "System Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Admin123!", Roles.Admin),

                (new ApplicationUser
                {
                    UserName = "manager@nanohealth.com",
                    Email = "manager@nanohealth.com",
                    FirstName = "Ali",
                    LastName = "Mahmoud",
                    Department = "Operations",
                    JobTitle = "Operations Manager",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Manager123!", Roles.Manager),

                (new ApplicationUser
                {
                    UserName = "manager2@nanohealth.com",
                    Email = "manager2@nanohealth.com",
                    FirstName = "Mostafa",
                    LastName = "Hassan",
                    Department = "Sales",
                    JobTitle = "Sales Manager",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Manager123!", Roles.Manager),

                (new ApplicationUser
                {
                    UserName = "finance@nanohealth.com",
                    Email = "finance@nanohealth.com",
                    FirstName = "Khalid",
                    LastName = "Ibrahim",
                    Department = "Finance",
                    JobTitle = "Finance Director",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Finance123!", Roles.Finance),

                (new ApplicationUser
                {
                    UserName = "finance2@nanohealth.com",
                    Email = "finance2@nanohealth.com",
                    FirstName = "Yasser",
                    LastName = "Omar",
                    Department = "Finance",
                    JobTitle = "Senior Accountant",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Finance123!", Roles.Finance),

                (new ApplicationUser
                {
                    UserName = "hr@nanohealth.com",
                    Email = "hr@nanohealth.com",
                    FirstName = "Kareem",
                    LastName = "Abdullah",
                    Department = "HR",
                    JobTitle = "HR Manager",
                    EmailConfirmed = true,
                    IsActive = true
                }, "HR123!", Roles.HR),

                (new ApplicationUser
                {
                    UserName = "employee@nanohealth.com",
                    Email = "employee@nanohealth.com",
                    FirstName = "Omar",
                    LastName = "Saleh",
                    Department = "Engineering",
                    JobTitle = "Software Developer",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Employee123!", Roles.Employee),

                (new ApplicationUser
                {
                    UserName = "employee2@nanohealth.com",
                    Email = "employee2@nanohealth.com",
                    FirstName = "Saeed",
                    LastName = "Al-Fahad",
                    Department = "Design",
                    JobTitle = "UI/UX Designer",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Employee123!", Roles.Employee),

                (new ApplicationUser
                {
                    UserName = "employee3@nanohealth.com",
                    Email = "employee3@nanohealth.com",
                    FirstName = "Faisal",
                    LastName = "Al-Zahra",
                    Department = "Marketing",
                    JobTitle = "Marketing Analyst",
                    EmailConfirmed = true,
                    IsActive = true
                }, "Employee123!", Roles.Employee)
            };

            foreach (var (user, password, role) in usersToCreate)
            {
                await CreateUserWithRoleAsync(userManager, user, password, role);
            }
        }

        private static async Task CreateUserWithRoleAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string password,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email!);

            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }
            }
        }
    }
}
