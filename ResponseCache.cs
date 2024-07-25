
// client side cash

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
[ResponseCache(Location = ResponseCacheLocation.Client, Duration = 300)]


[ResponseCache(CacheProfileName = "Caching")]
[ResponseCache(CacheProfileName = "NoCaching")]
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Caching",
        new CacheProfile()
        {
            Location = ResponseCacheLocation.Client,
            Duration = 300
        });
    options.CacheProfiles.Add("NoCaching",
        new CacheProfile()
        {
            Location = ResponseCacheLocation.None,
            NoStore = true
        });
});
