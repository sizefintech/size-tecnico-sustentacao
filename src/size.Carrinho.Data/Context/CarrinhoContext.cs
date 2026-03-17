using Microsoft.EntityFrameworkCore;
using size.Carrinho.Business.Entities;
using size.Core.Communication;
using size.Core.Data.Context;

namespace size.Carrinho.Data.Context
{
    public class CarrinhoContext : SizeContext, IDisposable
    {
        public CarrinhoContext(DbContextOptions<CarrinhoContext> options, INotificador notificador)
           : base(options, notificador)
        {
        }

        public DbSet<Duplicata> Duplicatas { get; set; }
        public DbSet<Business.AggregateRoots.Carrinho> Carrinhos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarrinhoContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
