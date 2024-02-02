using System.Linq.Expressions;
using ImmersiveView.Domain.Model.RepositoryAbstractions;
using ImmersiveView.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ImmersiveView.Repositories.Base;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly ImmersiveViewDataDbContext _dataDbContext;
    private readonly DbSet<TEntity> _dbEntitySet;

    public GenericRepository(ImmersiveViewDataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
        _dbEntitySet = _dataDbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbEntitySet
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbEntitySet
            .Where(predicate)
            .ToListAsync();
    }
    
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbEntitySet.FindAsync(id);
    }

    public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbEntitySet.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        TEntity? entity = await GetByIdAsync(id);
        return entity != null;
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbEntitySet.AddAsync(entity);
    }
    
    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbEntitySet.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _dbEntitySet.Attach(entity);
        _dataDbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        if (_dataDbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbEntitySet.Attach(entity);
        }

        _dbEntitySet.Remove(entity);
    }
}