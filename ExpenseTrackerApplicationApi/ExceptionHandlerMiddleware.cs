using ExpenseTrackerApplicationApi.Controllers.Exceptions;
using ExpenseTrackerApplicationApi.Database.Exceptions;
using System.Net;

namespace ExpenseTrackerApplicationApi
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline.
                await _next(context);
            }
            catch(InvalidUserException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            catch(EntryNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}
