using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.DAL.Repositories.AuthenticationRepo
{
    public interface IAuthentication
    {
        User Register(User user, string password);
        User Login(string userName, string password);
        bool UserExists(string userName, string phoneNumber = null);
    }
}
