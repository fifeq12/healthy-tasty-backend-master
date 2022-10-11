using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyTasty.Domain.Tables
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public class Mapping : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                builder.ToTable("category");
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.HasKey(x => x.Id);
            }
        }
    }
}
