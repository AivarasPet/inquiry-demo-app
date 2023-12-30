using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InquiryStatusUpdater.SignalRHubs
{
    [Authorize]
    public class NotifyHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(123);
            await base.OnConnectedAsync();
        }
    }
}
