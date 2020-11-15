using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniFB.BAL.FileUploadManager;
using MiniFB.DAL.UnitofWork;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Dtos;
using MiniFB.Infrastructure.Entities;
using MiniFB.Infrastructure.Enums;
using MiniFB.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniFB.BAL.PostManager
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IFileUpload uploadFile;
        public PostService(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IFileUpload uploadFile)
        {
            this.configuration = configuration;
            this.httpContext = httpContext;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.uploadFile = uploadFile;
        }
        public async Task<GenericOperationResult<bool>> AddPost(PostDto postDto, string host)
        {
            //Validate model
            var result = new GenericOperationResult<bool>();
            var postValidator = new PostValidator();
            var res = postValidator.Validate(postDto);
            if (res.IsValid)
            {
                try
                {
                    int userId = int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (postDto.Image != null)
                    {
                        var path = await uploadFile.UploadImage(postDto.Image, host);
                        if (path != null)
                        {
                            postDto.ImageUrl = path;
                        }
                        else
                        {
                            result.Messages.Add("Invalid image type");
                            result.Status = OperationResultStatusEnum.Exception;
                            return result;
                        }
                    }
                    Post createPost = MapPost(postDto);
                    createPost.UserId = userId;
                    unitOfWork.PostRepository.Create(createPost);
                    if (unitOfWork.SaveChanges())
                    {
                        result.Data = true;
                        result.Messages.Add("your post has been added ");
                        result.Status = OperationResultStatusEnum.Succeeded;
                        return result;
                    }
                    else
                    {
                        result.Data = false;
                        result.Messages.Add("Sorry your post couldn't be uploaded try again!!");
                        result.Status = OperationResultStatusEnum.Failed;
                        return result;
                    }
                }
                catch (NullReferenceException nullEx)
                {
                    throw new BusinessException("Null Refrence exception", nullEx);
                }
                catch (IndexOutOfRangeException outOfRangeEx)
                {
                    throw new BusinessException("Out of range exception exception", outOfRangeEx);
                }
                catch (SqlException sqlEx)
                {
                    throw new BusinessException("Sql Exception exception", sqlEx);
                }
                catch (Exception e)
                {
                    throw new BusinessException("error Occured ", e);
                }

            }

            result.Data = false;
            result.Messages = res.Errors.Select(e => e.ErrorMessage).ToList(); 
            result.Status = OperationResultStatusEnum.Failed;

            return result;
        }
        public GenericOperationResult<bool> DeletePost(int postId)
        {
            try
            {
                var result = new GenericOperationResult<bool>();
                int userId = int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var post = unitOfWork.PostRepository.GetById(postId);
                if (post != null && checkPostOwner(userId, post))
                {
                    unitOfWork.PostRepository.Delete(postId);
                    if (unitOfWork.SaveChanges())
                    {
                        result.Data = true;
                        result.Status = OperationResultStatusEnum.Succeeded;
                        result.Messages.Add("Your post has been deleted");
                        return result;
                    }

                    result.Data = false;
                    result.Status = OperationResultStatusEnum.Failed;
                    return result;
                }

                result.Data = false;
                result.Status = OperationResultStatusEnum.Failed;
                result.Messages.Add("Bad Data");
                return result;
            }
            catch (NullReferenceException nullEx)
            {
                throw new BusinessException("Null Refrence exception", nullEx);
            }
            catch (SqlException sqlEx)
            {
                throw new BusinessException("Sql Exception exception", sqlEx);
            }
            catch (Exception e)
            {
                throw new BusinessException("error Occured ", e);
            }

        }
        public GenericOperationResult<List<Post>> GetPosts()
        {
            try
            {
                GenericOperationResult<List<Post>> result = new GenericOperationResult<List<Post>>();
                int userId = int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userFriends = unitOfWork.UserFriendsRepository.Get().Where(u => u.UserId == userId);
                List<Post> posts = new List<Post>();
                foreach (var userfriend in userFriends)
                {
                    var friendPosts = unitOfWork.PostRepository.Get().Where(u => u.UserId == userfriend.FriendId).ToList();
                    foreach (var post in friendPosts)
                    {
                        posts.Add(post);
                    }

                }
                if (posts.Count > 0)
                {
                    result.Data = posts;
                    result.Messages.Add("Posts retrieved successfully");
                    result.Status = OperationResultStatusEnum.Succeeded;
                    return result;
                }
                result.Messages.Add("there's no posts");
                result.Status = OperationResultStatusEnum.Succeeded;
                return result;
            }
            catch (NullReferenceException nullEx)
            {
                throw new BusinessException("Null Refrence exception", nullEx);
            }
            catch (SqlException sqlEx)
            {
                throw new BusinessException("Sql Exception exception", sqlEx);
            }
            catch (Exception e)
            {
                throw new BusinessException("error Occured ", e);
            }

        }
        private bool checkPostOwner(int userId, Post post)
        {
            if(post.UserId == userId)
                return true;
            return false;
        }
        private Post MapPost(PostDto postDto)
        {
            var post = mapper.Map<Post>(postDto);
            return post;
        }
        public static void Mapping(Profile profile)
        {
            profile.CreateMap<PostDto, Post>();
        }

       
    }
    
}
