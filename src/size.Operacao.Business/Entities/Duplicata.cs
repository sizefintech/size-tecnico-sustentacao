using size.Core.DomainObjects;

namespace size.Operacao.Business.Entities
{
    public class Duplicata : Entity
    {
        public string Numero { get; private set; }
        public DateTime Vencimento { get; private set; }
        public decimal Valor { get; private set; }

        /*EF*/
        public AggregateRoots.Operacao Operacao { get; private set; }
        public string OperacaoId { get; private set; }

        private Duplicata() { }

        public Duplicata(string id, string numero, DateTime vencimento, decimal valor)
        {
            Id = id;
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        

        public int Prazo()
        {
            return (Vencimento - DateTime.Now).Days;
        }

        public decimal ValorLiquido()
        {
            return Valor * (1 - CalcularTaxaAntecipacao());
        }

        public decimal CalcularTaxaAntecipacao()
        {
            var diasParaVencimento = (Vencimento - DateTime.Now).Days;

            if (diasParaVencimento <= 30)
                return 0.02m;
            else if (diasParaVencimento <= 60)
                return 0.035m;
            else if (diasParaVencimento <= 90)
                return 0.05m;
            else
                return 0.08m;
        }

        internal void VincularOperacao(string id)
        {
            OperacaoId = id;
        }
    }
}
