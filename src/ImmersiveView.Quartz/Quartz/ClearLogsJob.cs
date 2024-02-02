using ImmersiveView.Domain.Model.InfrastructureServiceAbstractions;
using Quartz;

namespace ImmersiveView.Quartz.Quartz;

public class ClearLogsJob(ILogEventsService logEventsService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await logEventsService.TruncateLogEventsTableAsync();
        logEventsService.RemoveLogFiles();
    }
}