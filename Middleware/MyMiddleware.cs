namespace M.Middleware;

public class MyMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Items.Add("q", "in visited myMiddleware");
        await _next.Invoke(context);
    }
}
