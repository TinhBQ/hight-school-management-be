using Contexts;
using Services.Implementation.Extensions;

namespace API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, HsmsDbContext dbContext)
        {
            var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var userId = JwtUtils.ValidateToken(token ?? "");
            if (userId != Guid.Empty)
            {
                // attach user to context on successful jwt validation
                httpContext.Items["User"] = dbContext.Teachers.First(t => t.Id == userId).Username;
            }

            await _next(httpContext);
        }
    }
}
