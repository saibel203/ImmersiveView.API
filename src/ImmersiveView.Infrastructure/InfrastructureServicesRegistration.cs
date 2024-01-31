using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;

namespace ImmersiveView.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices<TFiltering>(this IServiceCollection services,
        IConfiguration configuration)
        where TFiltering: class
    {
        const string dataConnectionString = "DataConnectionString";
        const string logsTableName = "LogEvents";

        const string fromEmailConfiguration = "SmtpConfiguration:FromEmail";
        const string toEmailConfiguration = "SmtpConfiguration:ToEmail";
        const string subjectEmailConfiguration = "SmtpConfiguration:ToEmail";
        const string fromNameEmailConfiguration = "SmtpConfiguration:FromName";
        const string portEmailConfiguration = "SmtpConfiguration:Port";
        const string sslEmailConfiguration = "SmtpConfiguration:EnableSsl";
        const string applicationEmailPasswordConfiguration = "SmtpConfiguration:ApplicationPassword";

        const string sendGridApiKey = "SendGridOptions:ApiSecretKey";

        const string currentProjectName = "ApplicationsName:Infrastructure";
        const string logsPath = "Paths:LogPath";

        const string outputTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;


        if (projectDirectory is null)
            throw new DirectoryNotFoundException();

        string logFilesPath = Path.Combine(projectDirectory, configuration[currentProjectName] ?? string.Empty,
            configuration[logsPath] ?? string.Empty);

        MSSqlServerSinkOptions sinkOptions = new MSSqlServerSinkOptions
        {
            TableName = logsTableName,
            AutoCreateSqlTable = true
        };

        SendGridClient sendGridClient = new SendGridClient(configuration[sendGridApiKey]);

        NetworkCredential networkCredential = new NetworkCredential
        {
            UserName = configuration[fromEmailConfiguration],
            Password = configuration[applicationEmailPasswordConfiguration]
        };

        EmailConnectionInfo emailConnectionInfo = new EmailConnectionInfo
        {
            EmailSubject = configuration[subjectEmailConfiguration],
            FromEmail = configuration[fromEmailConfiguration],
            ToEmail = configuration[toEmailConfiguration],
            NetworkCredentials = networkCredential,
            SendGridClient = sendGridClient,
            FromName = configuration[fromNameEmailConfiguration],
            Port = configuration.GetValue<int>(portEmailConfiguration),
            EnableSsl = configuration.GetValue<bool>(sslEmailConfiguration)
        };

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.File(logFilesPath, rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information, rollOnFileSizeLimit: true,
                outputTemplate: outputTemplate)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: outputTemplate)
            .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString(dataConnectionString),
                sinkOptions: sinkOptions,
                restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.Email(emailConnectionInfo, restrictedToMinimumLevel: LogEventLevel.Error)
            .Filter.ByIncludingOnly(Matching.FromSource<TFiltering>())
            .CreateLogger();

        return services;
    }
}