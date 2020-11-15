using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniFB.BAL.UserManager;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Enums;
using MiniFB.Infrastructure.Models;

namespace MiniFB_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        public UserController(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        [HttpPost]
        public GenericOperationResult<IActionResult> AddFriend(AddFriendModel addFriend)
        {
            var result = new GenericOperationResult<IActionResult>();
            if(addFriend.UserName != "" || addFriend.UserName != null)
            {
                try
                {
                    return userService.AddFriend(addFriend.UserName);
                }
                catch (BusinessException ex)
                {
                    Logger.Error(ex, "error occured during adding friend");
                    result.Data = new BadRequestResult();
                    result.Messages.Add("Bad data can;t add friend try again ");
                    result.Status = OperationResultStatusEnum.Failed;
                    return result;
                }
            }
            result.Data = new BadRequestResult();
            result.Status = OperationResultStatusEnum.Failed;
            result.Messages .Add("userName can't be empty");
            return result;
        }

    }
}
