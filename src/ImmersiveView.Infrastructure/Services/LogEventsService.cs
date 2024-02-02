using ImmersiveView.Domain.Model.InfrastructureServiceAbstractions;
using ImmersiveView.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImmersiveView.Infrastructure.Services;

public class LogEventsService(ImmersiveViewDataDbContext dataDbContext, IConfiguration configuration)
    : ILogEventsService
{
    public async Task TruncateLogEventsTableAsync()
    {
        const string sqlStoredProcedureName = "[dbo].[Logs_Clear]";
        await dataDbContext.Database.ExecuteSqlRawAsync(sqlStoredProcedureName);
    }

    public void RemoveLogFiles()
    {
        const string currentProjectName = "ApplicationsName:Infrastructure";
        const string logFolderPath = "Paths:LogFolderPath";

        string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (projectDirectory is null)
            throw new DirectoryNotFoundException();

        string logFilesPath = Path.Combine(projectDirectory, configuration[currentProjectName] ?? string.Empty,
            configuration[logFolderPath] ?? string.Empty);

        DirectoryInfo directory = new DirectoryInfo(logFilesPath);
        
        foreach (FileInfo file in directory.GetFiles())
            file.Delete();
    }
}