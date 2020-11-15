using Microsoft.EntityFrameworkCore;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL
{
    public interface IMiniFbContext
    {
        DbSet<TEntity> Set<TEntity>()
           where TEntity : class;
        public int SaveChanges();
        public void Dispose();
        public  DbSet<User> Users { get; set; }
        public  DbSet<Post> Posts { get; set; }
        public  DbSet<UserFriends> UserFriends { get; set; }
    }
}
