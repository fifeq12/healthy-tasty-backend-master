using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthyTasty.Domain.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Domain
{
    public class HealthyTastyContext : DbContext
    {
        public HealthyTastyContext(DbContextOptions<HealthyTastyContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealthyTastyContext).Assembly);
        }
    }
}
