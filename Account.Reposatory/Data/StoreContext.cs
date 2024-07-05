using Account.Core.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Account.Reposatory.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }


        public DbSet<Item> items { get; set; }
        public DbSet<Persone>  persones { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Complains> complains { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Item>()
                 .HasKey(i => i.Id);

            modelBuilder.Entity<Persone>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Item>()
           .HasIndex(i => i.UniqNumber)
           .IsUnique();


            modelBuilder.Entity<Item>()
            .HasMany(i => i.Comments)
            .WithOne(c => c.Item)
            .HasForeignKey(c => c.ItemId);


            modelBuilder.Entity<Persone>()
            .HasMany(i => i.Comments)
            .WithOne(c => c.Person)
            .HasForeignKey(c => c.PersonId);

            modelBuilder.Entity<Item>()
           .HasOne(i => i.User)
           .WithMany(u => u.Items)
           .HasForeignKey(i => i.UserId)
           .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Item)
            .WithMany(i => i.Comments)
            .HasForeignKey(c => c.ItemId)
            .OnDelete(DeleteBehavior.Cascade);


        }




    }
}

