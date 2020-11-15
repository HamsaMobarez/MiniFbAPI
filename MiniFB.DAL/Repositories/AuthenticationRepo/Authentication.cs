using AutoMapper;
using Microsoft.Extensions.Configuration;
using MiniFB.DAL.UnitofWork;
using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MiniFB.DAL.Repositories.AuthenticationRepo
{
    public class Authentication : IAuthentication
    {
        
        private readonly IUnitOfWork unitOfWork;
        private readonly IMiniFbContext context;
        public Authentication(IMiniFbContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }
        public User Login(string userName, string password)
        {
            var user = unitOfWork.UserRepository.GetByUserName(userName);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                return null;

            return user;
        }

        public User Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;
            //unitOfWork.UserRepository.Create(user);
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var code = new HMACSHA512())
            {
                passwordSalt = code.Key;
                passwordHash = code.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var code = new HMACSHA512(passwordSalt))
            {
                var computedHash = code.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }
        public bool UserExists(string userName, string phoneNumber = null)
        {
            if (context.Users.Any(u => u.UserName == userName))
                return true;

            else if (context.Users.Any(u => u.PhoneNumber == phoneNumber))
                return true;
            return false;
        }

    }
}
