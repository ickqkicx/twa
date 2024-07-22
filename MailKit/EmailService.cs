using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;

namespace Account.Services;

public class EmailService
{
    private const string AppName = "Auth";
    private const string From = "...";
    public const string ConfirmSubject = "Link for confirm your account on app: Auth.";
    public const string PasswordResetSubject = "Link for password reset on app: Auth.";

    private const string Server = "smtp";
    private const int Port = 465;

    private const string UserName = "Xxx";
    private const string Password = "xxX";

    public async Task SendAsync(string to, string subject, string body)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(AppName, From));
        emailMessage.To.Add(new MailboxAddress(to, to));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(Server, Port, true);
        await client.AuthenticateAsync(UserName, Password);
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }
}
