using ImmersiveView.Domain.Model.RepositoryAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ImmersiveView.Repositories;

public static class ApplicationRepositoriesRegistration
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}