
Console.WriteLine("Hello asp.net");

// serilog.aspnetcore, serilog.sinks.async
string ot = "{Timestamp:HH:mm:ss}:{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}";
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);

Log.Logger = logger
    .WriteTo.Async(c => c.Console(outputTemplate: ot))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
            .WriteTo.Async(ou => ou.File("Logs/info-.txt", outputTemplate: ot, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning)
            .WriteTo.Async(ou => ou.File("Logs/warn-.txt", outputTemplate: ot, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
            .WriteTo.Async(ou => ou.File("Logs/error-.txt", outputTemplate: ot, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Fatal)
            .WriteTo.Async(ou => ou.File("Logs/fatal-.txt", outputTemplate: ot, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
    .CreateLogger();
