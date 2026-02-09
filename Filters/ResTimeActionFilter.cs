using Microsoft.AspNetCore.Mvc.Filters;

namespace capstone_prjct.Filters
{
    public class ResTimeActionFilter : IActionFilter
    {
        private const string RequestTimeKey = "RequestTime";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Capture the request time when action execution begins
            var requestTime = DateTime.UtcNow;
            context.HttpContext.Items[RequestTimeKey] = requestTime;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Capture the response time when action execution completes
            var responseTime = DateTime.UtcNow;

            if (context.HttpContext.Items[RequestTimeKey] is DateTime requestTime)
            {
                // Calculate the time difference
                var elapsedTime = responseTime - requestTime;
                var elapsedMilliseconds = elapsedTime.TotalMilliseconds;

                // Add response time to response headers
                context.HttpContext.Response.Headers.Add("X-Request-Time", requestTime.ToString("o"));
                context.HttpContext.Response.Headers.Add("X-Response-Time", responseTime.ToString("o"));
                context.HttpContext.Response.Headers.Add("X-Elapsed-Time", $"{elapsedMilliseconds}ms");

                // Log the response time
                // var logger = context.HttpContext.RequestServices.GetService<ILogger<ResTimeActionFilter>>();
                // logger?.LogInformation(
                //     "Request {Method} {Path} - RequestTime: {RequestTime}, ResponseTime: {ResponseTime}, Elapsed: {ElapsedMs}ms",
                //     context.HttpContext.Request.Method,
                //     context.HttpContext.Request.Path,
                //     requestTime,
                //     responseTime,
                //     elapsedMilliseconds
                // );
            }
        }
    }
}
