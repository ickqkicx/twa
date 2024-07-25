
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // CRIME and BREACH (for fix add random data in headers, ratelimiting, AntiforgeryToken)
	options.Providers.Clear();
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});


app.UseResponseCompression();
