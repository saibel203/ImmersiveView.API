using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImmersiveView.Persistence;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        const string dataConnectionString = "DataConnectionString";

        services.AddDbContext<ImmersiveViewDataDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString(dataConnectionString)));
        
        return services;
    }
}