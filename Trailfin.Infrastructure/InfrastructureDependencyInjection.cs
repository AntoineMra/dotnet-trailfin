using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trailfin.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Trailfin.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrailfinContext>(options =>
            {
                string conString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
                options.UseMySql(conString, ServerVersion.AutoDetect(conString));
            });
            return services;
        }
    }
}