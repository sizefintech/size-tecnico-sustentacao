using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.Core.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Extensions
{
    public static class ConfigurationExtensions
    {

        public static IServiceCollection AddSqlServerDb<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(configuration.ObterConnectionString(), x =>
                {
                    x.MaxBatchSize(1500);
                    x.CommandTimeout(180);
                }));

            return services;
        }

        public static string ObterConnectionString(this IConfiguration configuration)
        {
            var connectionString = configuration[ConfiguracaoStringDeConexao.DEFAULT_CONNECTION];
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Informe a ConnectionString. Caso esteja debugando, coloque-o na UserSecret ConnectionStrings:DefaultConnection");

            return connectionString;
        }
    }
}
