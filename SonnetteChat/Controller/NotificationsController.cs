using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sonnette.Chat.Hubs;
using Sonnette.Chat.Models;

namespace Sonnette.Chat.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        //Interface hubContext
        private readonly IHubContext<ChatHub> _hubContext;


        public NotificationsController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

/*      [HttpGet(Name = "GetTest")]
        public Notification[] Get()
        {
            return new Notification[]
            {
                new Notification
                {
                    idNotif = 1,
                    dateNotif = DateTime.Now,
                    typeNotif = 2
                },
                new Notification
                {
                    idNotif = 2,
                    dateNotif = DateTime.Now,
                    typeNotif = 4
                }
            };
        }*/

        [HttpPost]
        //Lire uniquement dans le body (Pas head, ni footer)
        public async Task<ActionResult> NotifSonnette([FromBody] Notification notif)
        {
            Console.WriteLine("On passe dans le POST");

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", notif);

            return NoContent();
        }

    }
}
