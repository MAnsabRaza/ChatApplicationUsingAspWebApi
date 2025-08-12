using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatApplication.Models
{
    public class AppDbContext : DbContext
    {
           public AppDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Permission> Permission { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Module>().ToTable("Modules");
            modelBuilder.Entity<Permission>().ToTable("Permissions");

            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.roleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasRequired(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.roleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasRequired(p => p.Module)
                .WithMany()
                .HasForeignKey(p => p.moduleId)
                .WillCascadeOnDelete(false);
        }
    }
}
