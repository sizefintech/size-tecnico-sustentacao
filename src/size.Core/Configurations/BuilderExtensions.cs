using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.Core.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Configurations
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddSizeCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificador, Notificador>();

            return services;
        }

    }
}
