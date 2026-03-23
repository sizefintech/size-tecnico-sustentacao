using Microsoft.EntityFrameworkCore;
using size.Core.Communication;
using size.Core.Data.Context;
using size.Operacao.Business.Entities;

namespace size.Operacao.Data.Context
{
    public class OperacaoContext : SizeContext, IDisposable
    {
        public OperacaoContext(DbContextOptions<OperacaoContext> options, INotificador notificador)
           : base(options, notificador)
        {
        }

        public DbSet<Business.AggregateRoots.Operacao> Operacoes { get; set; }
        public DbSet<Duplicata> Duplicatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OperacaoContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
