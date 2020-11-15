using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniFB.DAL.UnitofWork;

namespace MiniFB.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IMiniFbContext, MiniFbContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("MiniFbDB")));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
