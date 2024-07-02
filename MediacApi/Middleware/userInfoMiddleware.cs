using Serilog;
using System.Security.Claims;

namespace MediacApi.Middleware
{
    public class userInfoMiddleware
    {
        private readonly RequestDelegate next;
        private static int Count = 1;

        public userInfoMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User.FindFirst(ClaimTypes.Email)?.Value;
            DateTime dateTime = DateTime.Now;
            TimeSpan timeOnly = dateTime.TimeOfDay;
            await next(context);

            if(user == null)
            {
                Log.Debug($"some one get to the application to {context.Request.Path}");
                Count++;
            }
            else
            {
                Log.Debug($"{user} has get to {context.Request.Path} at request no. {Count}");
            }
        }
    }
}
