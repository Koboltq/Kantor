namespace Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Repository.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<OglContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OglContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //if(System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}
            SeedRoles(context);
            SeedUsers(context);
        }

        private void SeedRoles(OglContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            if(!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(OglContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);
            if(!context.Users.Any(x=>x.UserName == "Admin"))
            {
                var user = new User { UserName = "Admin", Age = 12 };
                var adminResult = manager.Create(user,"12345678");
                if (adminResult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }
            if (!context.Users.Any(u => u.UserName == "Marek"))
            {
                var user = new User { UserName = "marek@AspNetMvc.pl" };
                var adminResult = manager.Create(user, "1234Abc.");
                if (adminResult.Succeeded)
                    manager.AddToRole(user.Id, "Employee");
            }
            if(!context.Users.Any(u=>u.UserName == "President"))
            {
                var user = new User { UserName = "president@AspNetMvc.pl" };
                var adminResult = manager.Create(user, "1234Abc.");
                if (adminResult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }
        }

    }
}
