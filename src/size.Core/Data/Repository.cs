using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using size.Core.Data.Context;
using size.Core.DomainObjects;
using System.Linq.Expressions;

namespace size.Core.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        protected readonly SizeContext Db;
        protected readonly DbSet<TEntity> DbSet;
        public int QUANTIDADE_LIMITES_PADRAO = 50;

        public Repository(SizeContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual void Adicionar(TEntity entity)
        {
            entity.DefinirAtualizadoEm(DateTime.UtcNow);
            DbSet.Add(entity);
        }

        public virtual void Adicionar(IEnumerable<TEntity> entity)
        {
            entity.ToList().ForEach(x => { x.DefinirAtualizadoEm(DateTime.UtcNow); });
            DbSet.AddRange(entity);
        }

        public virtual void AdicionarPorBulk(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.DefinirAtualizadoEm(DateTime.UtcNow);
            }
            Db.BulkInsert(entities.ToList());
        }

        public virtual void Atualizar(TEntity entity)
        {
            entity.DefinirAtualizadoEm(DateTime.UtcNow);
            DbSet.Update(entity);
        }

        public virtual void Atualizar(IEnumerable<TEntity> entity)
        {
            entity.ToList().ForEach(x => { x.DefinirAtualizadoEm(DateTime.UtcNow); });
            DbSet.UpdateRange(entity);
        }

        public virtual void AtualizarPorBulk(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.DefinirAtualizadoEm(DateTime.UtcNow);
            }
            Db.BulkUpdate(entities.ToList());
        }

        public virtual void Excluir(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual TEntity ObterPorId(string id, params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = Db.Set<TEntity>().AsQueryable();

            if (includes == null)
                return dbSet.FirstOrDefault(x => x.Id == id);

            foreach (var include in includes)
                dbSet = dbSet.Include(include);

            return dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual TEntity ObterPorId(string id)
        {
            return DbSet
                .FirstOrDefault(x => x.Id == id);
        }

        public virtual TEntity Obter(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = Db.Set<TEntity>().AsQueryable();
            return dbSet.FirstOrDefault(predicate);
        }

        public virtual IEnumerable<TEntity> Listar(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = Db.Set<TEntity>().AsQueryable();
            return dbSet.Where(predicate).ToList();
        }

        public virtual IEnumerable<TEntity> ListarPorIds(IEnumerable<string> ids, params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = Db.Set<TEntity>().AsQueryable();

            if (includes == null)
                return dbSet.Where(t => ids.Contains(t.Id)).ToList();

            foreach (var include in includes)
                dbSet = dbSet.Include(include);

            return dbSet.Where(t => ids.Contains(t.Id)).ToList();
        }

        public bool VerificarSeExiste(string id)
        {
            return DbSet
                    .Any(x => x.Id == id);
        }

        public void Dispose()
        {
            //GC.SuppressFinalize(this);
            Db?.Dispose();
        }

        public void SalvarAlteracoes()
        {
            Db.SalvarAlteracoes();
        }
    }
}
