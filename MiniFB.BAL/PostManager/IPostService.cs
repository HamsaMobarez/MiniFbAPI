using Microsoft.AspNetCore.Http;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Dtos;
using MiniFB.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniFB.BAL.PostManager
{
    public interface IPostService
    {
        Task<GenericOperationResult<bool>> AddPost(PostDto postDto, string host);
        GenericOperationResult<bool> DeletePost(int postId);
        GenericOperationResult<List<Post>> GetPosts();
    }
}
