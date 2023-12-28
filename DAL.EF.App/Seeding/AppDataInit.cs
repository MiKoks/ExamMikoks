using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Seeding;

public class AppDataInit
{
    private static Guid adminId = Guid.Parse("bc7458ac-cbb0-4ecd-be79-d5abf19f8c77");
    
    public static void MigrateDatabase(ApplicationDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DropDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
    }
    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        (Guid id, string email, string pwd, string Admin) userData = (adminId, "admin@app.com", "Foo.bar.1", "admin");
        var user = userManager.FindByEmailAsync(userData.email).Result;
        if (user == null)
        {
            user = new AppUser()
            {
                Id = userData.id,
                Email = userData.email,
                UserName = userData.email,
                FirstName = "Admin",
                LastName = "App",
                EmailConfirmed = true,
            };
            var result = userManager.CreateAsync(user, userData.pwd).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result.ToString()}");
            }
        }
        SeedRoles(roleManager);
        AddNewUser(userManager, roleManager, new UserData(){Id = adminId, Email = "admin@app.com", Password = "Foo.bar.1",FirstName = "admin",LastName = "app", RoleName = "admin"});
    }
    
    private class UserData
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }

    
    private static void AddNewUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        UserData userData)
    {
        var user = userManager.FindByEmailAsync(userData.Email).Result;

        if (user == null)
        {
            user = new AppUser()
            {
                Id = userData.Id,
                Email = userData.Email,
                UserName = userData.Email,
                EmailConfirmed = true,
            };

            var result = userManager.CreateAsync(user, userData.Password).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result.ToString()}");
            }
        }

        

        if (!string.IsNullOrWhiteSpace(userData.RoleName))
        {
            var role = roleManager.FindByNameAsync(userData.RoleName).Result;
            if (role == null)
            {
                var identityResult = roleManager.CreateAsync(new AppRole()
                {
                    Name = userData.RoleName,
                }).Result;

                if (!identityResult.Succeeded)
                {
                    throw new ApplicationException($"Role creation failed [{userData.RoleName}]");
                }
            }


            if (!string.IsNullOrWhiteSpace(userData.RoleName))
            {
                var identityResultRole =
                    (userManager.AddToRolesAsync(user, new List<string>() { userData.RoleName })).Result;
            }
        }
    }
    
    
    
    private static void SeedRoles(RoleManager<AppRole> roleManager)
    {
        CreateRoleIfNotExists(roleManager, "admin");
        CreateRoleIfNotExists(roleManager, "user");
    }
    
    private static void CreateRoleIfNotExists(RoleManager<AppRole> roleManager, string roleName)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            var role = new AppRole { Name = roleName };
            var result = roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed role '{roleName}', {result}");
            }
        }
    }
    
      
    
    
}