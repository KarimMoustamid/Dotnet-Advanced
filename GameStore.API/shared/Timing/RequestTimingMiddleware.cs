namespace GameStore.API.shared.Timing
{
    using System.Diagnostics;

    public class RequestTimingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ILogger<RequestTimingMiddleware> logger)
        {

            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                await next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                logger.LogInformation("Request {requestMethod} {requestPath} completed in {elapsedMilliseconds}ms with status {status}", context.Request.Method, context.Request.Path, elapsedMilliseconds, context.Response.StatusCode);
            }
        }
    }
}