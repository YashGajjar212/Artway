using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Artway.Application.Exceptions;

namespace Artway.Presentation.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CustomException customException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = customException.StatusCode,
                    Title = customException.ErrorCode,
                    Detail = customException.Message,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = customException.StatusCode;
                await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                // Exception handled Successfully
                return true;
            }
            // Let ASP.NET Core handle any other exceptions
            return false;
        }
    }
}