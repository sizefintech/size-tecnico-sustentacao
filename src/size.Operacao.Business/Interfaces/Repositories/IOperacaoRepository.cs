using size.Core.Data;

namespace size.Operacao.Business.Interfaces.Repositories
{
    public interface IOperacaoRepository : IRepository<Entities.Operacao>
    {
        Task<Entities.Operacao> ObterPorCodigo(string codigo);
        Task<Entities.Operacao> ObterOperacaoComDuplicatas(string operacaoId);
        Task<IEnumerable<Entities.Operacao>> ObterPorTomadorId(string tomadorId);
        void Adicionar(Entities.Operacao operacao);
        void Atualizar(Entities.Operacao operacao);
        void Remover(Entities.Operacao operacao);
    }
}
