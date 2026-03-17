using size.Core.Data;

namespace size.Operacao.Business.Interfaces.Repositories
{
    public interface IOperacaoRepository : IRepository<AggregateRoots.Operacao>
    {
        Task<AggregateRoots.Operacao> ObterPorCodigo(string codigo);
        Task<AggregateRoots.Operacao> ObterOperacaoComDuplicatas(string operacaoId);
        Task<IEnumerable<AggregateRoots.Operacao>> ObterPorTomadorId(string tomadorId);
        void Adicionar(AggregateRoots.Operacao operacao);
        void Atualizar(AggregateRoots.Operacao operacao);
        void Remover(AggregateRoots.Operacao operacao);
    }
}
