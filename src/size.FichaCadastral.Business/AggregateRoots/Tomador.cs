using size.Core.DomainObjects;
using size.Core.DomainObjects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.FichaCadastral.Business.AggregateRoots
{
    public class Tomador : Entity, IAggregateRoot
    {
        public string RazaoSocial { get; private set; }
        public Documento Documento { get; private set; }

    }
}
