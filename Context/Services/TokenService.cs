
using System.Collections.Concurrent;
using Account.Models;

namespace Account.Services;

public class TokenService
{
    private readonly ConcurrentQueue<Token> _tokens = [];
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromMinutes(10);

    public string GenerateConfirmationToken() => GenerateToken(TokenType.Confirmation);
    public string GeneratePasswordResetToken() => GenerateToken(TokenType.PasswordReset);

    private string GenerateToken(TokenType type)
    {
        var token = new Token(Guid.NewGuid().ToString(),
            DateTime.Now.Add(_tokenLifetime), type);
        _tokens.Enqueue(token);

        return token.ToString();
    }

    public bool IsValidConfirmationToken(string token) => IsValidToken(token, TokenType.Confirmation);
    public bool IsValidPasswordResetToken(string token) => IsValidToken(token, TokenType.PasswordReset);

    private bool IsValidToken(string value, TokenType type)
    {
        var token = _tokens.FirstOrDefault(t => t.Expiry >= DateTime.Now 
                            && t.Value == value && t.Type == type);

        return token != null;
        //_tokens = new ConcurrentQueue<Token>(_tokens.Where(t => t != token));
    }

    internal void CleanupExpiredTokens()
    {
        while (_tokens.TryPeek(out var token) && token.Expiry <= DateTime.Now)
        {
            _tokens.TryDequeue(out _);
        }
    }
}
