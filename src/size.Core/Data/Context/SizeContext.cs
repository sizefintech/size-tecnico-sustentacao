using Microsoft.EntityFrameworkCore;
using Polly;
using size.Core.Communication;
using size.Core.Communication.Notificacoes;
using System;

namespace size.Core.Data.Context
{
    public class SizeContext : DbContext, IDisposable
    {

        private readonly INotificador _notificador;

        public SizeContext(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected SizeContext(DbContextOptions options, INotificador notificador) : base(options)
        {
            _notificador = notificador;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(200)");

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(double) || p.ClrType == typeof(decimal))))
                property.SetColumnType("decimal(12,2)");

            modelBuilder.HasDefaultSchema(GetType().Name.Replace("Context", ""));
        }

        public void SalvarAlteracoes()
        {
            try
            {
                SalvarAlteracoesDb();
            }
            catch (Exception e)
            {
                _notificador.Notificar(new Notificacao("Tivemos um erro ao gravar algumas informações. Por favor, tente novamente em instantes."));
            }
        }

        private void SalvarAlteracoesDb()
        {
            var dbPolicy = ObterPolicyDb();
            dbPolicy.Execute(SaveChanges);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }


        private Policy ObterPolicyDb()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetry(TentativasPolicy.Db,
                                       retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

    }
}

public static class TentativasPolicy
{
    public static int Db = 3;
}
