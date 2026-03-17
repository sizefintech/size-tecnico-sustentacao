using size.CatalogoRecebiveis.Business.AggregateRoots;
using size.Core.Data;
using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.CatalogoRecebiveis.Business.Interfaces
{
    public interface IDuplicataRepository : IRepository<Duplicata>, IAggregateRoot
    {
    }
}
