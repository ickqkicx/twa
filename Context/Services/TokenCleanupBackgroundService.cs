
namespace Auth.Services;

public class TokenCleanupBackgroundService(TokenService tokenService) : BackgroundService
{
    private readonly TokenService _tokenService = tokenService;

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _tokenService?.CleanupExpiredTokens();
            await Task.Delay(1000 * 60 * 12, cancellationToken);
        }
    }
}

builder.Services.AddHostedService<TokenCleanupBackgroundService>();
