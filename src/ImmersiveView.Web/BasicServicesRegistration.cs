namespace ImmersiveView.Web;

public static class BasicServicesRegistration
{
    public static IServiceCollection AddBasicServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
        
        return services;
    }
}