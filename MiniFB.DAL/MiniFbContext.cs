using Microsoft.EntityFrameworkCore;
using MiniFB.DAL.Configurations;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL
{
   public class MiniFbContext : DbContext, IMiniFbContext
    {
        public MiniFbContext(DbContextOptions<MiniFbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get ; set; }
        public virtual DbSet<Post> Posts { get ; set; }
        public virtual DbSet<UserFriends> UserFriends { get; set ; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new PostConfig());
            modelBuilder.ApplyConfiguration(new UserFriendsConfig());
        }
    }
}
