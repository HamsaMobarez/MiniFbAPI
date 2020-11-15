using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MiniFB.DAL;
using System.Reflection;
using AutoMapper;
using MiniFB.BAL.Mappings;
using Twilio.Clients;
using MiniFB.BAL.Twilio;
using MiniFB.BAL.UserManager;
using System;
using MiniFB.BAL.FileUploadManager;
using MiniFB.BAL.PostManager;

namespace MiniFB.BAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDAL(configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileUpload, FileUpload>();
            services.AddScoped<IPostService, PostService>();
            services.AddHttpClient<ITwilioRestClient, CustomTwilioClient>();
            return services;
        }
    }
}
