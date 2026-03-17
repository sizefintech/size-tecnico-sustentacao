using size.Core.DomainObjects;
using size.Operacao.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Operacao.Business.AggregateRoots
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

        public Operacao(string tomadorId, List<Duplicata> duplicatas)
        {
            TomadorId = tomadorId;
            AdicionarDuplicatas(duplicatas);
            TaxaAntecipacao = _duplicatas.Sum(x=> x.CalcularTaxaAntecipacao());
            Prazo =(int)_duplicatas.Average(x=> x.Prazo()) ;
            DataCriacao = DateTime.Now;
            ValorBruto = _duplicatas.Sum(x => x.Valor);
            ValorLiquido = _duplicatas.Sum(x => x.ValorLiquido());
            VincularOperacaoAsDuplicatas();
        }

        private void VincularOperacaoAsDuplicatas()
        {
            _duplicatas.ForEach(d => d.VincularOperacao(Id));
        }

        public void VincularOperacaoDuplicatas()
        {
            _duplicatas.ForEach(x => x.VincularDuplicataOperacao(Id));
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

        private void AdicionarDuplicatas(List<Duplicata> duplicatas)
        {            
            _duplicatas.AddRange(duplicatas);
        }
    }
}
