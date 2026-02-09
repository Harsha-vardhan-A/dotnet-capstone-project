using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace capstone_prjct.Filters;

public class GlobalResponseFilter: IAsyncResultFilter
{
    // Implementation of the global response filter
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Logic to modify the response globally
        var originalResult = context.Result;
        if (originalResult is ObjectResult objectResult)
        {
            var modifiedResponse = new
            {
                Status = objectResult.StatusCode >= 200 && objectResult.StatusCode < 300,
                Data = objectResult.Value,
                Message = objectResult.StatusCode >= 200 && objectResult.StatusCode < 300 ? "Request successful" : "Request failed",
                Timestamp = DateTime.UtcNow,
                TraceId = context.HttpContext.TraceIdentifier,
            };
            context.Result = new ObjectResult(modifiedResponse)
            {
                StatusCode = objectResult.StatusCode
            };
        }
        
        await next();
    }

}