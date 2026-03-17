using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Operacao.Business.Entities
{
    public class Operacao : Entity, IAggregateRoot
    {
        public string TomadorId { get; private set; }
        public string Codigo { get; private set; }
        public decimal ValorBruto { get; private set; }
        public decimal ValorLiquido { get; private set; }
        public decimal TaxaAntecipacao { get; private set; }
        public int Prazo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataProcessamento { get; private set; }

        private List<Duplicata> _duplicatas = new List<Duplicata>();
        public IReadOnlyCollection<Duplicata> Duplicatas => _duplicatas;

        private Operacao() { }

        public Operacao(string tomadorId, string codigo, decimal taxaAntecipacao, int prazo)
        {
            TomadorId = tomadorId;
            Codigo = codigo;
            TaxaAntecipacao = taxaAntecipacao;
            Prazo = prazo;
            DataCriacao = DateTime.Now;
        }

        public void AdicionarDuplicata(Duplicata duplicata)
        {
            if (_duplicatas.Any(d => d.Numero == duplicata.Numero))
                throw new DomainException("Duplicata já existe na operação");

            _duplicatas.Add(duplicata);
            RecalcularValores();
        }

        public void AdicionarDuplicatas(IEnumerable<Duplicata> duplicatas)
        {
            foreach (var duplicata in duplicatas)
            {
                _duplicatas.Add(duplicata);
            }
            RecalcularValores();
        }

        public void ProcessarOperacao()
        {
            if (DataProcessamento.HasValue)
                throw new DomainException("Operação já foi processada");

            if (!_duplicatas.Any())
                throw new DomainException("Não é possível processar uma operação sem duplicatas");

            DataProcessamento = DateTime.Now;
        }

        private void RecalcularValores()
        {
            ValorBruto = _duplicatas.Sum(d => d.Valor);
            ValorLiquido = ValorBruto * (1 - TaxaAntecipacao);
        }
    }

    public class Duplicata : Entity
    {
        public string Numero { get; private set; }
        public DateTime Vencimento { get; private set; }
        public decimal Valor { get; private set; }

        /*EF*/
        public Operacao Operacao { get; private set; }
        public string OperacaoId { get; private set; }

        private Duplicata() { }

        public Duplicata(string numero, DateTime vencimento, decimal valor, string operacaoId)
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
            OperacaoId = operacaoId;
        }
    }
}
