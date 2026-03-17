using size.CatalogoRecebiveis.Business.AggregateRoots;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.CatalogoRecebiveis.Data.Context;
using size.Core.Data;
using size.Core.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.CatalogoRecebiveis.Data.Repositories
{
    public class DuplicataRepository : Repository<Duplicata>, IDuplicataRepository
    {
        public DuplicataRepository(CatalogoRecebiveisContext db) : base(db)
        {
        }
    }
}
