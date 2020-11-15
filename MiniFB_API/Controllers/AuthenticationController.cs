using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniFB.BAL.UserManager;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Dtos;
using MiniFB.Infrastructure.Enums;
using MiniFB.Infrastructure.Models;
using System.Net.Http;

namespace MiniFB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IUserService userService;
        private readonly IConfiguration config;
        public AuthenticationController(IUserService userService, IConfiguration config)
        {
            this.userService = userService;
            this.config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public GenericOperationResult<bool> Register(RegisterDto register)
        {
            var result = new GenericOperationResult<bool>();
           result = userService.RegisterUser(register);
            return result;
        }
        [HttpPost]
        [AllowAnonymous]

        public GenericOperationResult<LoginReturnDto> VerifyPhoneNumber(VerifyMobileModel model)
        {
            var result = new GenericOperationResult<LoginReturnDto>();
            return userService.VerifyUserPhone(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public GenericOperationResult<LoginReturnDto> Login(LoginDto loginDto)
        {
            return userService.LoginUser(loginDto);
        }
    }
}
