using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Configurations
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(P => P.Id);
            builder.Property(P => P.Id).UseIdentityColumn();
            builder.Property(p => p.Text).IsRequired().HasMaxLength(250);
            builder.HasOne(p => p.User).WithMany(u => u.UserPosts).HasForeignKey(fk => fk.UserId);
        }
    }
}
