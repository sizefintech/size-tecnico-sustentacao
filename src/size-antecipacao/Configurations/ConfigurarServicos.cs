using size.Core.Configurations;

namespace size_antecipacao.Configurations
{
    public static class ConfigurarServicos
    {
        public static void AdicionarServicos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AdicionarServicoes(configuration);

            services.AddSizeCore(configuration);
        }
    }
}
