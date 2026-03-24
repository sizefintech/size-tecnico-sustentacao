using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.Operacao.Application.Services;
using size.Operacao.Business.Interfaces.Repositories;
using size.Operacao.Data.Context;
using size.Operacao.Data.Repositories;
using size.Core.Data.Config;

namespace size.Operacao.Configurations
{
    public static class BuilderExtensions
    {
        public static void AddOperacao(this IServiceCollection services, IConfiguration config)
        {
            services.AddSqlServerDb<OperacaoContext>(config);

            services.AddScoped<IOperacaoRepository, OperacaoRepository>();
            services.AddScoped<OperacaoService>();
        }

        internal static IServiceCollection AddSqlServerDb<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
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
