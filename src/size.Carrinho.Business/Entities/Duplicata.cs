using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using size.Carrinho.Business.AggregateRoots;

namespace size.Carrinho.Business.Entities
{
    public class Duplicata : Entity
    {
        [JsonIgnore]
        public Business.AggregateRoots.Carrinho Carrinho { get; private set; }
        public string CarrinhoId { get; private set; }
        private Duplicata() { }
        public Duplicata(string id, string carrinhoId)
        {
            Id = id;
            CarrinhoId = carrinhoId;
        }

        public void SetarCarrinho(string carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }
    }
}
