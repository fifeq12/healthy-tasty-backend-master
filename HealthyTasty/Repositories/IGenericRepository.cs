using System.Linq.Expressions;
using AutoMapper.Execution;
using HealthyTasty.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HealthyTasty.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        public readonly HealthyTastyContext Context;
        private readonly DbSet<TEntity> _entity;

        public GenericRepository(HealthyTastyContext context)
        {
            Context = context;
            _entity = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await GetBy(predicate, null, cancellationToken);
        }

        public async Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
            CancellationToken cancellationToken)
        {
            var result = _entity.Where(predicate);

            if (include != null)
                result = include(result);

            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return await _entity.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await GetAll(predicate, null, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAll(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            CancellationToken cancellationToken)
        {
            return await GetAll(null, include, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
            CancellationToken cancellationToken)
        {
            var result = _entity.Where(x => true);

            if (predicate != null)
                result = result.Where(predicate);

            if (include != null)
                result = include(result);

            return await result.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithLimit(int limit, Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await GetAllWithLimit(limit, predicate, null, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithLimit(int limit, Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
            CancellationToken cancellationToken)
        {
            var result = _entity.Where(x => true);

            if (predicate != null)
                result = result.Where(predicate);

            if (include != null)
                result = include(result);

            return await result.Take(limit).ToListAsync(cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }

        public void Create(TEntity entity)
        {
            _entity.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public Task SaveChanges(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }

    public interface IGenericRepository<TEntity>
    {
        Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllWithLimit(int limit, Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllWithLimit(int limit, Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
            CancellationToken cancellationToken);

        void Update(TEntity entity);
        void Create(TEntity entity);
        void Delete(TEntity entity);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
