
Console.WriteLine("Hello asp.net");

builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
});