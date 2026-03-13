namespace DotNet10Demo.Filters;

class LoggingFilter(ILogger<LoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var path = context.HttpContext.Request.Path;
        
        logger.LogInformation("Request: {Method} {Path}", context.HttpContext.Request.Method, path);

        var result = await next(context);

        logger.LogInformation("Response: {StatusCode} for {Path}", context.HttpContext.Response.StatusCode, path);

        return result;
    }
}
