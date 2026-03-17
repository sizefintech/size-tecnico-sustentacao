using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.Core.Extensions;
using size.fichaCadastral.Data.Context;
using size.fichaCadastral.Data.Repositories;
using size.FichaCadastral.Business.Interfaces.Repositories;

namespace size.FichaCadastral.Configurations
{
    public static class BuilderExtensions
    {
        public static void AddFichaCadastralConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddSqlServerDb<FichaCadastralContext>(config);

            services.AddScoped<ITomadorRepository, TomadorRepository>();
        }

       

        
    }


}


