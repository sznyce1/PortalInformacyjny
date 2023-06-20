using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.entities
{
    public class ArticleDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=ArticleDb;Trusted_Connection=True;";
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();
            modelBuilder.Entity<Role>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Article>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);
            modelBuilder.Entity<Article>()
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(5000);
            modelBuilder.Entity<Category>()
                .Property(p=> p.Name)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Comment>()
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(1000);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
