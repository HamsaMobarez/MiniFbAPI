using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Repositories.UserFriendsRepo
{
    public class UserFriendsRepository: Repository<UserFriends>, IUserFriendsRepository
    {
        public UserFriendsRepository(IMiniFbContext context) : base(context)
        {
        }
    }
}
