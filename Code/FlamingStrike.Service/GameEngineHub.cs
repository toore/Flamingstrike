using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    //[Authorize]
    public class GameEngineHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SendAction", "user", "joined");

            //await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("SendAction", "user", "left");

            //await Clients.All.SendAsync("SendAction", Context.User.Identity.Name, "left");
        }
    }
}