using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs;

public class GewichtHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("Client verbonden: " + Context.UserIdentifier);
        await base.OnConnectedAsync();
    }
}