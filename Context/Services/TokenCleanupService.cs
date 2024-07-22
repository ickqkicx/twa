
namespace Account.Services;

public class TokenCleanupService(TokenService tokenService) : IHostedService, IDisposable
{
    private readonly TokenService _tokenService = tokenService;
    private Timer? _timer = null;

    private void DoWork(object? state)
    {
        _tokenService.CleanupExpiredTokens();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(12));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
