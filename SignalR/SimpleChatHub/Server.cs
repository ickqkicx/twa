using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs;

public record Message(string UserName, string Text, DateTime Time);

public class SimpleChatHub : Hub
{
    public async Task SendToGeneral(Message message)
    {
        await Clients.Others.SendAsync("ReceiveFromGeneral", message);
    }
}


//app.MapHub<SimpleChatHub>("/sch");
