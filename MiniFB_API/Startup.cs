using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniFB.BAL;
using MiniFB.Infrastructure.Common;
using MiniFB.Infrastructure.Enums;
using NLog;

namespace MiniFB_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection("Token"));
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var errors = c.ModelState.Values.Where(v => v.Errors.Count > 0)
                                                      .SelectMany(v => v.Errors)
                                                      .Select(v => v.ErrorMessage)
                                                      .ToList();

                    return new OkObjectResult(new GenericOperationResult<object>()
                    {
                        Status = OperationResultStatusEnum.Validation,
                        Messages = errors,
                    });
                };
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Token:Secrete").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
            services.AddHttpContextAccessor();
            services.AddAuthorization();
            services.AddBAL(Configuration);
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddCors();
            //services.AddMvc(c => c.Conventions.Add(new ApiExplorerIgnores()));
            services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Mini FaceBook" });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini FaceBook V1");
                c.RoutePrefix = string.Empty;
            });
      
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCors(builder =>
                   builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
        }
    }
}
