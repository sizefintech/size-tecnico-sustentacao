using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.DomainObjects
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
            CriadoEm = AtualizadoEm = DateTime.UtcNow;
        }

        public string Id { get; protected set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime AtualizadoEm { get; private set; }
        public void DefinirAtualizadoEm(DateTime atualizadoAs)
        {
            AtualizadoEm = atualizadoAs;
        }

    }
}
