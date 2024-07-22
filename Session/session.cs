
Console.WriteLine("Hello asp.net");

var tempSession = HttpContext.Session.GetString("tempSession");
httpContext.Session.SetString("tempSession", "123");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});



app.UseSession();