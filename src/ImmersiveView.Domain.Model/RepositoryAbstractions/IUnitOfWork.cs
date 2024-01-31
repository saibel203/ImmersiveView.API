namespace ImmersiveView.Domain.Model.RepositoryAbstractions;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    IGenericRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;
}