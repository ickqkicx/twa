namespace M.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder
            .UseMiddleware<MyMiddleware>()
            .UseMiddleware<CheckerMyMiddleware>();

        // hehe
        return builder
            .UseMiddleware<CheckerMyMiddleware>()
            .UseMiddleware<MyMiddleware>();
    }
}

//app.UseCustomMiddleware();
