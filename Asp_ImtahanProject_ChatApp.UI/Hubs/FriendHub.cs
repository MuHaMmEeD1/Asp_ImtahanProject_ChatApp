using Microsoft.AspNetCore.SignalR;

namespace Asp_ImtahanProject_ChatApp.UI.Hubs
{
    public class FriendHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Kullanıcıların bağlanması
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Kullanıcıların ayrılması
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
