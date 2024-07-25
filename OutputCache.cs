
// server side cash

builder.Services.AddOutputCache(o =>
{
    o.SizeLimit = 64 * 1024 * 1024;
    o.MaximumBodySize = 4 * 1024 * 1024;
    o.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(1);
});


app.UseOutputCache();
