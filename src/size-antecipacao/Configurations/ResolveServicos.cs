using size.ApplicationService.ProcessamentoCheckout;
using size.ApplicationService.ProcessamentoCheckout.Interfaces;
using size.Carrinho.Application.Services;
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



            services.AddScoped<IProcessamentoCheckoutAppService, ProcessamentoCheckoutAppService>();
            services.AddScoped<CheckoutService>();

            return services;
        }
    }
}
