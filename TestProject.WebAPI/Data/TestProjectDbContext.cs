using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Data
{
    public class TestProjectDbContext : DbContext
    {
        public TestProjectDbContext(DbContextOptions<TestProjectDbContext> options)
: base(options)
        {
            //    ChangeTracker.LazyLoadingEnabled = false;
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });
        }
        public class DbContextFactory : IDesignTimeDbContextFactory<TestProjectDbContext>
        {
            public TestProjectDbContext CreateDbContext(string[] args)
            {
                var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrWhiteSpace(envName))
                    throw new ArgumentException(
                        "No ASPNETCORE_ENVIRONMENT has been specified. Type in package manager console $env:ASPNETCORE_ENVIRONMENT='EnvName'.");

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(Directory.GetCurrentDirectory() + "/../TestProject.WebAPI/appsettings.json",
                        false, true)
                    .AddJsonFile(
                        Directory.GetCurrentDirectory() + $"/../TestProject.WebAPI/appsettings.{envName}.json",
                        true, true).Build();
                var builder = new DbContextOptionsBuilder<TestProjectDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlServer(connectionString);
                return new TestProjectDbContext(builder.Options);
            }
        }
    }
}
