using size.Core.DomainObjects;
using System.Linq.Expressions;

namespace size.Core.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity, IAggregateRoot
    {
        void Adicionar(TEntity entity);
        void Adicionar(IEnumerable<TEntity> entity);
        void Atualizar(TEntity entity);
        void Atualizar(IEnumerable<TEntity> entity);
        void Excluir(TEntity entity);
        bool VerificarSeExiste(string id);
        void SalvarAlteracoes();
        TEntity Obter(Expression<Func<TEntity, bool>> predicate);
        TEntity ObterPorId(string id);
        TEntity ObterPorId(string id, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> Listar(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> ListarPorIds(IEnumerable<string> ids, params Expression<Func<TEntity, object>>[] includes);
        void AdicionarPorBulk(IEnumerable<TEntity> entities);
        void AtualizarPorBulk(IEnumerable<TEntity> entities);
    }
}
