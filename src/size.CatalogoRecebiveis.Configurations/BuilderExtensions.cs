

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.CatalogoRecebiveis.Data.Context;
using size.CatalogoRecebiveis.Data.Repositories;
using size.Core.Extensions;

namespace size.CatalogoRecebiveis.Configurations
{
    public static class BuilderExtensions
    {
        public static void AddCatalogoRecebiveis(this IServiceCollection services, IConfiguration config)
        {
            services.AddSqlServerDb<CatalogoRecebiveisContext>(config);

            services.AddScoped<IDuplicataRepository, DuplicataRepository>();
        }
     
    }
}
