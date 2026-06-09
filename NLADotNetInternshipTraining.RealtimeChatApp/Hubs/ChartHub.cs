using Microsoft.AspNetCore.SignalR;

namespace NLADotNetInternshipTraining.RealtimeChatApp.Hubs;

public class ChartHub : Hub
{
    // When a client connects, we can optionally send them the current data
    // but it's simpler to send it via controller on page load, or a hub method.
    // Let's allow clients to request initial data.
    public async Task GetInitialData()
    {
        // The controller or hub will call this, but the hub can access the static data.
        // Let's implement static data in a shared service or controller.
    }
}