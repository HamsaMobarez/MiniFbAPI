using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniFB.DAL.Repositories.UserRepo
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);
        User GetByPhoneNumber(string PhoneNumber);
    }
}
