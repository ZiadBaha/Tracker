using Account.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Account.Reposatory.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        // This method is called when the model is being created. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This line searches for any classes that implement the 
            // IEntityTypeConfiguration interface within the current assembly 
            SeedRoles(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "User", ConcurrencyStamp = "1", NormalizedName = "User" },
                new IdentityRole { Name = "BussinesOwner", ConcurrencyStamp = "2", NormalizedName = "BussinesOwner" },
                new IdentityRole { Name = "ServiceProvider", ConcurrencyStamp = "3", NormalizedName = "ServiceProvider" },
                new IdentityRole { Name = "Admin", ConcurrencyStamp = "4", NormalizedName = "Admin" }
            );
        }

    }

}
