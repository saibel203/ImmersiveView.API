using ImmersiveView.Quartz.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ImmersiveView.Quartz;

public static class QuartzServicesRegistration
{
    public static IServiceCollection AddQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartzOptions =>
        {
            quartzOptions.AddJobWithTrigger<ClearLogsJob>(configuration);
        });

        services.AddQuartzHostedService(quartzOptions =>
        {
            quartzOptions.WaitForJobsToComplete = true;
        });
        
        return services;
    }
}