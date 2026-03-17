using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace size.CatalogoRecebiveis.Business.AggregateRoots
{
    public class Duplicata : Entity, IAggregateRoot
    {
        public string TomadorId { get; private set; }
        public string Numero { get; private set; }
        public decimal Valor { get; private set; }
        public decimal ValorLiquido { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public int Status { get; private set; }
        public bool NoCarrinho { get; private set; }

        public void MarcarComoNoCarrinho()
        {
            NoCarrinho = true;
        }

        public void RemoverDoCarrinho()
        {
            NoCarrinho = false;
        }

        public void AtualizarValorLiquido(decimal valorLiquido)
        {
            ValorLiquido = valorLiquido;
        }

        public void AtualizarStatus(int status)
        {
            Status = status;
        }
    }
}
