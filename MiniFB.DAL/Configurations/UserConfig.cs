using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Configurations
{
    class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(U => U.Id);
            builder.Property(U => U.Id).UseIdentityColumn();
            builder.Property(U => U.Password).IsRequired();
        }
    }
}
