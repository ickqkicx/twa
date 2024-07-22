
Console.WriteLine("Hello asp.net");

var cookieOptions = new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.None,
    MaxAge = TimeSpan.FromDays(1),
};
httpContext.Response.Cookies.Append("tempCookie", "123", cookieOptions);