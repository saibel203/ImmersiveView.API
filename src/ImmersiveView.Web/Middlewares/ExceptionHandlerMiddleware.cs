﻿using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace ImmersiveView.Web.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly ILogger _logger = Log.ForContext<ExceptionHandlerMiddleware>();
    private readonly string _messageTemplate = "HTTP {Domain} {RequestMethod} {StatusCode} {RequestPath}";

    public async Task InvokeAsync(HttpContext httpContext)
    {
        HostString domain = httpContext.Request.Host;
        Claim? username = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Username");

        Stream originalResponseBody = httpContext.Response.Body;

        try
        {
            await using MemoryStream responseBodyStream = new MemoryStream();

            httpContext.Response.Body = responseBodyStream;

            await next(httpContext);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

            int statusCode = httpContext.Response.StatusCode;
            if (statusCode > 399)
            {
                string headers = GetHeaders(httpContext);

                _logger.Write(LogEventLevel.Error,
                    $"\n\n{_messageTemplate}\n\n{username}\n\n{responseBody}\n\n{headers}",
                    domain, httpContext.Request.Method, httpContext.Response.StatusCode,
                    httpContext.Request.Path);
            }

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
        catch (Exception ex)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (httpContext.Response != null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            string headers = GetHeaders(httpContext);

            _logger.Write(LogEventLevel.Fatal,
                $"\n\n{_messageTemplate}\n\n{username}\n\n{ex.Message}\n\n{ex.StackTrace}\n\n{headers}",
                domain, httpContext.Request.Method, httpContext.Response?.StatusCode, httpContext.Request.Path);

            throw;
        }
        finally
        {
            if (httpContext.Response != null)
            {
                httpContext.Response.Body = originalResponseBody;
            }
        }
    }

    private string GetHeaders(HttpContext httpContext)
    {
        StringBuilder sb = new StringBuilder();
        string separator = new String('=', 20);

        sb.AppendLine("Request Headers");
        foreach (KeyValuePair<string, StringValues> header in httpContext.Request.Headers)
        {
            sb.AppendLine(separator);
            sb.AppendLine($"{header.Key}: {header.Value}");
        }

        sb.AppendLine();
        sb.AppendLine("Response Headers");
        foreach (KeyValuePair<string, StringValues> header in httpContext.Response.Headers)
        {
            sb.AppendLine(separator);
            sb.AppendLine($"{header.Key}: {header.Value}");
        }

        return sb.ToString();
    }
}