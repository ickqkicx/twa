
new WebApplicationOptions { WebRootPath = "wwwroot" } // default folder static files
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=600";
    }
});

