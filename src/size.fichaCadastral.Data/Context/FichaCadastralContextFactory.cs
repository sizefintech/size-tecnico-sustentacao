using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using size.Core.Communication;
using size.Core.Communication.Notificacoes;
using size.Core.Data.Config;

namespace size.fichaCadastral.Data.Context
{
    public class FichaCadastralContextFactory : IDesignTimeDbContextFactory<FichaCadastralContext>
    {
        public FichaCadastralContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("7a5daa8b-f09e-4e36-863c-2b38cd330803")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FichaCadastralContext>();
            
            var connectionString = configuration[ConfiguracaoStringDeConexao.DEFAULT_CONNECTION];
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Connection string not found. Please configure ConnectionStrings:DefaultConnection in user secrets.");

            optionsBuilder.UseSqlServer(connectionString, x =>
            {
                x.MaxBatchSize(1500);
                x.CommandTimeout(180);
            });

            var notificador = new Notificador();
            
            return new FichaCadastralContext(optionsBuilder.Options, notificador);
        }
    }
}
