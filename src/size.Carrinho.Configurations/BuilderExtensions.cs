using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using size.Carrinho.Application.Services;
using size.Carrinho.Business.Interfaces.Repositories;
using size.Carrinho.Data.Context;
using size.Carrinho.Data.Repositories;
using size.Core.Data.Config;

namespace size.Carrinho.Configurations
{
    public static class BuilderExtensions
    {
        public static void AddCarrinho(this IServiceCollection services, IConfiguration config)
        {
            services.AddSqlServerDb<CarrinhoContext>(config);

            services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
            services.AddScoped<CheckoutService>();
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
