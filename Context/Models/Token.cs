namespace Account.Models;

public class Token(string value, DateTime expiry, TokenType type)
{
    public readonly string Value = value;
    public readonly DateTime Expiry = expiry;
    public readonly TokenType Type = type;

    public override string ToString() => Value;
}

public enum TokenType
{
    Confirmation = 0,
    PasswordReset = 1,
}