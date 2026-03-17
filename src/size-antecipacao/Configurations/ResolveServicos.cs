using size.Carrinho.Configurations;
using size.CatalogoRecebiveis.Configurations;
using size.FichaCadastral.Configurations;
using size.Operacao.Configurations;

namespace size_antecipacao.Configurations
{
    public static class ResolveServicos
    {

        public static IServiceCollection AdicionarServicoes(this IServiceCollection services, IConfiguration config)
        {
            services.AddFichaCadastralConfigurations(config);
            services.AddCatalogoRecebiveis(config);
            services.AddCarrinho(config);
            services.AddOperacao(config);
            return services;
        }
    }
}
