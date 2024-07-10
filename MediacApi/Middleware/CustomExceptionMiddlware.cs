using MediacApi.HelperClasses;
using MediacApi.Services.IRepositories;
using Serilog;
using System.Net;

namespace MediacApi.Middleware
{
    public class CustomExceptionMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly ihttpAccessor context;

        public CustomExceptionMiddlware(RequestDelegate next)
        {
            this._next = next;
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                Log.Error($"Something went wrong: {ex}");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(new ErrorDetails
            {
                Message = "Internal server error from custom middleware",
                StatusCode = context.Response.StatusCode
            }.ToString());
        }
    }
}
