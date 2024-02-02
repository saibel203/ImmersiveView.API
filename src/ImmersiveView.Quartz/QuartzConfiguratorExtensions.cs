using Microsoft.Extensions.Configuration;
using Quartz;

namespace ImmersiveView.Quartz;

public static class QuartzConfiguratorExtensions
{
    public static void AddJobWithTrigger<T>(this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration configuration)
        where T : IJob
    {
        string jobName = typeof(T).Name;
        string identityName = $"{jobName}-trigger";

        string configurationKey = $"QuartzConfiguration:{jobName}";
        string? cronSchedule = configuration[configurationKey];

        if (string.IsNullOrWhiteSpace(cronSchedule))
            throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configurationKey}");

        JobKey jobKey = new JobKey(jobName);
        quartz.AddJob<T>(options => options.WithIdentity(jobKey));

        quartz.AddTrigger(options => options
            .ForJob(jobKey)
            .WithIdentity(identityName)
            .WithCronSchedule(cronSchedule));
    }
}