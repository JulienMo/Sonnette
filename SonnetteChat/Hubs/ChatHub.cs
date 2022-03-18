using System;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using Sonnette.Chat.Models;

namespace Sonnette.Chat.Hubs
{
    public class ChatHub : Hub
    {
/*        public async Task SendMessage(string name, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", name, message);
        }*/

        public async Task SendMessage(Notification notif)
        {
            await Clients.All.SendAsync("ReceiveMessage", "toté", "fdsdsf59");
        }
    }
}


