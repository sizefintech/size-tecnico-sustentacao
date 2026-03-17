using Microsoft.EntityFrameworkCore;
using size.CatalogoRecebiveis.Business.AggregateRoots;
using size.Core.Communication;
using size.Core.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace size.CatalogoRecebiveis.Data.Context
{    

    public class CatalogoRecebiveisContext : SizeContext, IDisposable
    {
        public CatalogoRecebiveisContext(DbContextOptions<CatalogoRecebiveisContext> options, INotificador notificador)
            : base(options, notificador)
        {
        }

        public DbSet<Duplicata> Duplicatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoRecebiveisContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
