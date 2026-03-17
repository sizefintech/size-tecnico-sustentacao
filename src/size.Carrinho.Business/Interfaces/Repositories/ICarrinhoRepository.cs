using size.Core.Data;

namespace size.Carrinho.Business.Interfaces.Repositories
{
    public interface ICarrinhoRepository : IRepository<AggregateRoots.Carrinho>
    {
        Task<AggregateRoots.Carrinho> ObterPorAgregateId(string agregateId);
        Task<AggregateRoots.Carrinho> ObterCarrinhoComDuplicatas(string carrinhoId);
        void Adicionar(AggregateRoots.Carrinho carrinho);
        void Atualizar(AggregateRoots.Carrinho carrinho);
        void Remover(AggregateRoots.Carrinho carrinho);
    }
}
