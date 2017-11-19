namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Linkofy.Models;


    internal sealed class Configuration : DbMigrationsConfiguration<Linkofy.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Linkofy.Models.ApplicationDbContext";
        }

        protected override void Seed(Linkofy.Models.ApplicationDbContext context)
        {

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = new ApplicationUser { UserName = "liam@hotmail.co.uk" };
            userManager.Create(user, "test11@");
        }
        }
    }