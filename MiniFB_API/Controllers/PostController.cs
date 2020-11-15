using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniFB.BAL.PostManager;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Dtos;
using MiniFB.Infrastructure.Entities;
using MiniFB.Infrastructure.Enums;
using MiniFB.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniFB_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IConfiguration configuration;
        private readonly IPostService postService;
        public PostController(IConfiguration configuration, IPostService postService)
        {
            this.configuration = configuration;
            this.postService = postService;
        }

        [HttpPost]
        public async Task<GenericOperationResult<bool>> AddPost([FromForm]PostDto post)
        {
            GenericOperationResult<bool> result = new GenericOperationResult<bool>();
            try
            {
                var host = Request.Scheme + "://" + Request.Host;
                result = await postService.AddPost(post, host);
                return result;
            }
            catch(BusinessException ex)
            {
                Logger.Error(ex, "Error occured during adding new post");
                result.Data = false;
                result.Messages.Add("Bad Data");
                result.Status = OperationResultStatusEnum.Failed;
                return result;
            }
        }

        [HttpPost]
        public GenericOperationResult<bool> DeletePost(PostModel post)
        {
            GenericOperationResult<bool> result = new GenericOperationResult<bool>();
            if (post.PostId != null)
            {
                try
                {
                    return postService.DeletePost(int.Parse(post.PostId));
                }
                catch (BusinessException ex)
                {
                    Logger.Error(ex, "Error occured during deleting new post");
                    result.Data = false;
                    result.Messages.Add("Bad Data");
                    result.Status = OperationResultStatusEnum.Failed;
                    return result;
                }
            }
            result.Messages.Add("Invalid post id input");
            result.Status = OperationResultStatusEnum.Failed;
            return result;
           
        }
        [HttpGet]
        public GenericOperationResult<List<Post>> GetUserPosts()
        {
            var result = new GenericOperationResult<List<Post>>();
            try
            {
                return postService.GetPosts();
            }
            catch (BusinessException ex)
            {
                Logger.Error(ex, "Error occured during getting posts");
                result.Messages.Add("Bad Data");
                result.Status = OperationResultStatusEnum.Failed;
                return result;
            }
        }
    }
}
