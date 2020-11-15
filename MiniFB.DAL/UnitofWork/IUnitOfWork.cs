using MiniFB.DAL.Repositories.AuthenticationRepo;
using MiniFB.DAL.Repositories.PostRepo;
using MiniFB.DAL.Repositories.UserFriendsRepo;
using MiniFB.DAL.Repositories.UserRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.UnitofWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        IUserFriendsRepository UserFriendsRepository { get;  }
        IAuthentication Authentication { get; }
        bool SaveChanges();
    }
}
