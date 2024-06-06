namespace M.Middleware;

public class CheckerMyMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items.ContainsKey("q"))
        {
            await _next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("myMiddleware was not be visit");
        }
    }
}
