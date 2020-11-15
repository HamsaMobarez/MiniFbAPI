using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Configurations
{
    class UserFriendsConfig : IEntityTypeConfiguration<UserFriends>
    {
        public void Configure(EntityTypeBuilder<UserFriends> builder)
        {
            builder.HasKey(Uf => new { Uf.UserId, Uf.FriendId });
            builder.HasOne(U => U.User).WithMany(F => F.UserFriends).HasForeignKey(Uf => Uf.UserId);
            builder.HasOne(F => F.Friend).WithMany(U => U.FriendsUsers).HasForeignKey(Uf => Uf.FriendId);
        }
    }
}
