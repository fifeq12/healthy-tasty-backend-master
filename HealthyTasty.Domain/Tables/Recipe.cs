using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyTasty.Domain.Tables
{
    public class Recipe : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual Category Category { get; set; }

        public class Mapping : IEntityTypeConfiguration<Recipe>
        {
            public void Configure(EntityTypeBuilder<Recipe> builder)
            {
                builder.ToTable("recipe");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.HasOne(x => x.Category);
            }
        }
    }
}
