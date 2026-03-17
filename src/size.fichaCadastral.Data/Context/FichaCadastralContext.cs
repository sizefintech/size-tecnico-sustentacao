using Microsoft.EntityFrameworkCore;
using size.Core.Communication;
using size.Core.Data.Context;
using size.FichaCadastral.Business.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.fichaCadastral.Data.Context
{
    public class FichaCadastralContext : SizeContext, IDisposable
    {
        public FichaCadastralContext(DbContextOptions<FichaCadastralContext> options, INotificador notificador) 
            : base(options, notificador)
        {
        }

        public DbSet<Tomador> Tomadores{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FichaCadastralContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
