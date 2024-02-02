using ImmersiveView.Domain.Model.RepositoryAbstractions;
using ImmersiveView.Persistence;

namespace ImmersiveView.Repositories.Base;

public class UnitOfWork(ImmersiveViewDataDbContext dateDbContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    private bool _disposed;

    public int SaveChanges()
    {
        return dateDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dateDbContext.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dateDbContext.SaveChangesAsync(cancellationToken);
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
            return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];

        GenericRepository<TEntity> repository = new GenericRepository<TEntity>(dateDbContext);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            dateDbContext.Dispose();

            foreach (IDisposable repository in _repositories.Values)
            {
                repository.Dispose();
            }
        }

        _disposed = true;
    }
}