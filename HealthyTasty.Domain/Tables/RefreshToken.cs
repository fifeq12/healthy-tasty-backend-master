using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyTasty.Domain.Tables
{
    public class RefreshToken : Entity
    {
        public string Token { get; set; }
        public bool Revoked { get; set; }
        public virtual User User { get; set; }

        public class Mapping : IEntityTypeConfiguration<RefreshToken>
        {
            public void Configure(EntityTypeBuilder<RefreshToken> builder)
            {
                builder.ToTable("refresh_token");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.HasOne(x => x.User);
            }
        }
    }
}
