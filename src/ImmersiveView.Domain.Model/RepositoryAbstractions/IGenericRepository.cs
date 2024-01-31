using System.Linq.Expressions;

namespace ImmersiveView.Domain.Model.RepositoryAbstractions;

public interface IGenericRepository<TEntity> 
    where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(int id);
    Task InsertAsync(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}