namespace SaaS.AcademicInstitutions.IdentityServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SaaS.AcademicInstitutions.IdentityServer.Infrastructure;
    using SaaS.AcademicInstitutions.IdentityServer.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SaaS.AcademicInstitutions.IdentityServer.Infrastructure.ApplicationIdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SaaS.AcademicInstitutions.IdentityServer.Infrastructure.ApplicationIdentityContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var manager = new UserManager<AcademicUser>(new UserStore<AcademicUser>(new ApplicationIdentityContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationIdentityContext()));
            var user = new AcademicUser()
            {
                UserName = "SuperPowerUser",
                Email = "Admin.Admin@test.com",
                EmailConfirmed = true,
                FirstName = "SuperPower",
                LastName = "User",
                Level = 3,
                JoinDate = DateTime.Now.AddYears(-3)
            };
            manager.Create(user, "MySuperP@ss!");
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("SuperPowerUser");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
        }

    }
    
}
