using MiniFB.Infrastructure.Entities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace MiniFB.DAL.Repositories.UserRepo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IMiniFbContext miniFbContext;
        public UserRepository(IMiniFbContext context): base(context)
        {
            this.miniFbContext = context;
        }

        public User GetByPhoneNumber(string PhoneNumber)
        {
            return miniFbContext.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber);
        }

        public User GetByUserName(string userName)
        {
            return miniFbContext.Users.FirstOrDefault(u => u.UserName == userName);
        }

       
    }
}
