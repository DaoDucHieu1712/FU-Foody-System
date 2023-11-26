using FFS.Application.Entities;
using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Hubs
{
    public class NotificationHub: Hub
    {
		public async Task SendNotification(Notification notification)
		{
			await Clients.All.SendAsync("ReceiveNotification", notification);
		}
		
		
	}
}
