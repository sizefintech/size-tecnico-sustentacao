using size.Carrinho.Business.Entities;
using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Carrinho.Business.AggregateRoots
{
    public class Carrinho : Entity, IAggregateRoot
    {
        public string AgregateId { get; private set; }
        public DateTime? InicioProcessamento { get; private set; }
        private List<Duplicata> _duplicatas = new List<Duplicata>();
        public IReadOnlyCollection<Duplicata> Duplicatas => _duplicatas;

        private Carrinho() { }

        public Carrinho(string agregateId)
        {
            AgregateId = agregateId;
        }

        public bool ExisteDuplicata(string duplicataId) => Duplicatas.Any(x => x.Id == duplicataId);
        public bool EstaProcessando() => InicioProcessamento.HasValue;

        public void Limpar()
        {
            _duplicatas.Clear();
            InicioProcessamento = null;
        }

        public void InserirDuplicata(Duplicata duplicata)
        {
            if (ExisteDuplicata(duplicata.Id))
                throw new DomainException("Duplicata ja existe no carrinho");

            _duplicatas.Add(duplicata);
        }

        public void InserirDuplicatas(IEnumerable<string> duplicatasId)
        {
            foreach (var duplicataId in duplicatasId)
                InserirDuplicata(new Duplicata(duplicataId, Id));
        }

        public void RemoverDuplicata(string duplicataId)
        {
            if (!ExisteDuplicata(duplicataId))
                throw new DomainException("Duplicata não existe no carrinho");

            var duplicata = _duplicatas.First(x => x.Id == duplicataId);
            _duplicatas.Remove(duplicata);
        }

        public void RemoverDuplicatas(IEnumerable<string> duplicataIds)
        {
            var duplicatasParaRemover = _duplicatas
                .Where(x => duplicataIds.Contains(x.Id))
                .ToList();

            foreach (var duplicata in duplicatasParaRemover)
            {
                _duplicatas.Remove(duplicata);
            }
        }

        public void IniciarProcessamento()
        {
            InicioProcessamento = DateTime.Now;
        }

        public void LimparProcessamento()
        {
            InicioProcessamento = null;
        }

    }
}
