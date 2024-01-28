using ImmersiveView.Identity;
using ImmersiveView.Infrastructure;
using ImmersiveView.Persistence;
using ImmersiveView.Web;
using ImmersiveView.Web.Middlewares;
using ImmersiveView.Web.Middlewares.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddIdentityServices(configuration);
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddInfrastructureServices<ExceptionHandlerMiddleware>(configuration);
builder.Services.AddBasicServices();

WebApplication app = builder.Build();

app.UseExceptionMiddleware();
app.MapControllers();

app.Run();