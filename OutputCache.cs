
// server side cash
using Microsoft.AspNetCore.OutputCaching;

[OutputCache]

builder.Services.AddOutputCache(o =>
{
    o.SizeLimit = 64 * 1024 * 1024;
    o.MaximumBodySize = 4 * 1024 * 1024;
    o.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(1);
});
// can be use with IDistributedCache (redis and ets...)


app.UseOutputCache();
