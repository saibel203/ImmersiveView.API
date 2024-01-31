namespace ImmersiveView.Web.Middlewares.Extensions;

public static class ExceptionHandlerMiddlewareExtension
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}