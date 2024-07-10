using MediacApi.HelperClasses;
using MediacApi.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Runtime.CompilerServices;

namespace MediacApi.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (context != null)
                    {
                        Log.Error($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            Message = "Internal Server Error",
                            StatusCode = context.Response.StatusCode
                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CustomExceptionMiddlware>();
        }
    }
}
