using Microsoft.AspNetCore.Mvc;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Dtos;
using MiniFB.Infrastructure.Entities;
using MiniFB.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiniFB.BAL.UserManager
{
    public interface IUserService
    {
        GenericOperationResult<bool> RegisterUser(RegisterDto register);
        GenericOperationResult<LoginReturnDto> LoginUser(LoginDto login);
        GenericOperationResult<LoginReturnDto> VerifyUserPhone(VerifyMobileModel model);
        GenericOperationResult<IActionResult> AddFriend(string userName); 
    }
}
