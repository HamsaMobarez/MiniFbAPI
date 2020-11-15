using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MiniFB.DAL.UnitofWork;
using MiniFB.Infrastructure.Entities;
using MiniFB.Infrastructure.Dtos;
using System;
using MiniFB.Infrastructure.Common;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using MiniFB.Infrastructure.Enums;
using MiniFB.Infrastructure.Models;
using Twilio.Types;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using System.Runtime.Caching;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MiniFB.Infrastructure.Validators;
using System.Linq;

namespace MiniFB.BAL.UserManager
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContext;
        private readonly ITwilioRestClient twilioClient;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContext, ITwilioRestClient client)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.httpContext = httpContext;
            this.twilioClient = client;
        }

        public GenericOperationResult<LoginReturnDto> LoginUser(LoginDto user)
        {
            var result = new GenericOperationResult<LoginReturnDto>();
            var loginValidator = new LoginValidator();
            var valid = loginValidator.Validate(user);
            if (!valid.IsValid)
            {
                result.Messages = valid.Errors.Select(e => e.ErrorMessage).ToList();
                result.Status = OperationResultStatusEnum.Validation;
                return result;
            }
            var loggedUser = unitOfWork.Authentication.Login(user.UserName, user.Password);
            if (loggedUser == null)
            {
                result.Messages.Add("Username or password are incorrect");
                result.Status = OperationResultStatusEnum.Exception;
                return result;
            }
                
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, loggedUser.UserName),
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token:Secrete").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Data = new LoginReturnDto { Token = tokenHandler.WriteToken(token)};
            result.Status = OperationResultStatusEnum.Succeeded;

            return result;
            ;
        }
            
  

        public GenericOperationResult<bool> RegisterUser(RegisterDto user)
        {
            var registerValidator = new RegisterValidator();
            var res = registerValidator.Validate(user);
            var result = new GenericOperationResult<bool>();
            if (res.IsValid) 
            {
                try
                {
                    if (!unitOfWork.Authentication.UserExists(user.UserName, user.PhoneNumber))
                    {
                        var registerUser = MapUser(user);
                        User registeredUser = new User();
                        registeredUser = unitOfWork.Authentication.Register(registerUser, user.Password);
                        VerifyUserModel model = new VerifyUserModel
                        {
                            User = registeredUser,
                            Password = user.Password
                        };
                        if (registeredUser != null)
                        {
                            httpContext.HttpContext.Session.SetString(user.PhoneNumber, JsonConvert.SerializeObject(model));
                            MessageModel messageModel = new MessageModel
                            {
                                From = new PhoneNumber("+12284564506"),
                                //Twilio license
                                To = new PhoneNumber("+201121736295"),
                                Body = "Verification code of your account is: 0000"
                            };
                            var message = SendMessage(messageModel);
                            //ObjectCache cache = MemoryCache.Default;
                            //cache.Add("CachedValueKey", "Some value to cache", new CacheItemPolicy()); 
                            //cache.Add(user.PhoneNumber, user.Password, new CacheItemPolicy());
                            result.Status = OperationResultStatusEnum.Succeeded;
                            result.Messages.Add("Suceeded");
                            return result;
                        }
                        result.Status = OperationResultStatusEnum.Failed;
                        result.Messages.Add("Failed");
                        return result;
                    }
                    else
                    {
                        result.Status = OperationResultStatusEnum.Failed;
                        result.Messages.Add("User already exists");
                        return result;
                    }

                }
                catch (ArgumentNullException e)
                {
                    throw new BusinessException("Argument is null ", e);
                }
                catch (SqlException e)
                {
                    throw new BusinessException("Database error ", e);
                }
                catch (NullReferenceException e)
                {
                    throw new BusinessException("Object Refrence is null ", e);
                }
                catch (Exception e)
                {
                    throw new BusinessException("error Occured ", e);
                }
            }
            result.Messages = res.Errors.Select(e => e.ErrorMessage).ToList();
            result.Status = OperationResultStatusEnum.Validation;
            return result;

        }
        public GenericOperationResult<LoginReturnDto> VerifyUserPhone(VerifyMobileModel model)
        {
            var result = new GenericOperationResult<LoginReturnDto>();
            var validator = new VerifyMobileValidator();
            var valid = validator.Validate(model);
            if (!valid.IsValid)
            {
                result.Messages = valid.Errors.Select(e => e.ErrorMessage).ToList();
                result.Status = OperationResultStatusEnum.Validation;
                return result;
            }
            if(model.OTP == "0000")
            {
                try
                {
                    if(httpContext.HttpContext.Session.GetString(model.PhoneNumber) == null)
                    {
                        result.Messages.Add("Invalid phone number Input");
                        result.Status = OperationResultStatusEnum.Exception;
                        return result;
                    }
                       
                    var user = httpContext.HttpContext.Session.GetString(model.PhoneNumber);
                    var myUser = JsonConvert.DeserializeObject<VerifyUserModel>(user);
                    if (myUser != null)
                    {
                        LoginDto loginData = new LoginDto
                        {
                            UserName = myUser.User.UserName,
                            Password = myUser.Password
                        };
                        httpContext.HttpContext.Session.Remove(model.PhoneNumber);
                        unitOfWork.UserRepository.Create(myUser.User);
                        unitOfWork.SaveChanges();
                        var res = LoginUser(loginData);
                        result.Data = new LoginReturnDto { Token = res.Data.Token};
                        result.Status = OperationResultStatusEnum.Succeeded;
                        return result;
                    }
                    result.Status = OperationResultStatusEnum.Failed;
                    return result;
                }
                catch (ArgumentNullException e)
                {
                    throw new BusinessException("Argument is null ", e);
                }
                catch (SqlException e)
                {
                    throw new BusinessException("Database error ", e);
                }
                catch (NullReferenceException e)
                {
                    throw new BusinessException("Object Refrence is null ", e);
                }
                catch (Exception e)
                {
                    throw new BusinessException("error Occured ", e);
                }
            }
            result.Status = OperationResultStatusEnum.Failed;
            result.Messages.Add("Invalid OTP");
            return result;
        }
        public GenericOperationResult<IActionResult> AddFriend(string userName)
        {
            var result = new GenericOperationResult<IActionResult>();
            try
            {
                int userId = int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (unitOfWork.Authentication.UserExists(userName) && userId != default)
                {
                    var friend = unitOfWork.UserRepository.GetByUserName(userName);
                    var user = unitOfWork.UserRepository.GetById(userId);
                    UserFriends userFriend = new UserFriends
                    {
                        FriendId = friend.Id,
                        UserId = userId,
                        Friend = friend,
                        User = user
                    };
                    unitOfWork.UserFriendsRepository.Create(userFriend);
                    if (unitOfWork.SaveChanges())
                    {
                        result.Data = new OkResult();
                        result.Messages.Add("Your friend has been added");
                        result.Status = OperationResultStatusEnum.Succeeded;
                        return result;
                    }
                    result.Data = new BadRequestResult();
                    result.Messages.Add("Insucessful process");
                    result.Status = OperationResultStatusEnum.Failed;
                    return result;
                }
            }
            catch (ArgumentNullException e)
            {
                throw new BusinessException("Argument is null ", e);
            }
            catch (SqlException e)
            {
                throw new BusinessException("Database error ", e);
            }
            catch (NullReferenceException e)
            {
                throw new BusinessException("Object Refrence is null ", e);
            }
            catch (Exception e)
            {
                throw new BusinessException("error Occured ", e);
            }
            result.Data = new BadRequestResult();
            result.Messages.Add("Friend userName doesn't exist");
            result.Status = OperationResultStatusEnum.Failed;
            return result;
        }

        private User MapUser(RegisterDto registerUser)
        {
            var user = mapper.Map<User>(registerUser);
            return user;
        }
        public static void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, User>().ForMember(x => x.Password, opt => opt.Ignore()).ReverseMap();
        }
        private string SendMessage(MessageModel model)
        {
            var message = MessageResource.Create(
                to: model.To,
                from: model.From,
                body: model.Body,
                client: twilioClient);
            return message.Sid;
        }

       
    }
}
