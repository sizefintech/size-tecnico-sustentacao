using size.Core.Data;
using size.fichaCadastral.Data.Context;
using size.FichaCadastral.Business.AggregateRoots;
using size.FichaCadastral.Business.Interfaces.Repositories;

namespace size.fichaCadastral.Data.Repositories
{
    public class TomadorRepository : Repository<Tomador>, ITomadorRepository
    {
        public TomadorRepository(FichaCadastralContext db) : base(db) { }
    }
}
