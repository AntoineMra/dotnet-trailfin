using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trailfin.Application.interfaces;
using Trailfin.Application.Services;
using System.Reflection;

namespace Trailfin.Application
{
    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}