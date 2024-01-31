using ImmersiveView.Application.AutomapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace ImmersiveView.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BaseMapperProfile));
        return services;
    }
}