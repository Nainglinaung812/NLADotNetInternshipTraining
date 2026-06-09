using Microsoft.AspNetCore.SignalR;

namespace NLADotNetInternshipTraining.RealtimeChatApp.Hubs;

public class ChatHub : Hub
{
    // server listen event
    public async Task ServerReceiveMessage(string user, string message)
    {
        // server hit event
        // receive message client listen event
        await Clients.All.SendAsync("ClientReceiveMessage", user, message);
        //await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}