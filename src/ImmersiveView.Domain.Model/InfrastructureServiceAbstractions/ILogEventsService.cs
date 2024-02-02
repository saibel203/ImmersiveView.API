namespace ImmersiveView.Domain.Model.InfrastructureServiceAbstractions;

public interface ILogEventsService
{
    Task TruncateLogEventsTableAsync();
    void RemoveLogFiles();
}