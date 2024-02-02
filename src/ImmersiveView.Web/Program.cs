using ImmersiveView.Application;
using ImmersiveView.Identity;
using ImmersiveView.Infrastructure;
using ImmersiveView.Persistence;
using ImmersiveView.Quartz;
using ImmersiveView.Repositories;
using ImmersiveView.Web;
using ImmersiveView.Web.Middlewares;
using ImmersiveView.Web.Middlewares.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddIdentityServices(configuration);
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddInfrastructureServices<ExceptionHandlerMiddleware>(configuration);
builder.Services.AddRepositoryServices();
builder.Services.AddApplicationServices();
builder.Services.AddQuartzServices(configuration);
builder.Services.AddBasicServices();

WebApplication app = builder.Build();

app.UseExceptionMiddleware();

app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();