using FFS.Application.Entities;
using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Hubs
{
    public class NotificationHub: Hub
    {
		
		public async Task JoinGroup(string userId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, userId);
		}
		public async Task SendNotification(Notification notification, string userId)
		{
			await Clients.Group(userId).SendAsync("ReceiveNotification", notification);
		}

	}
}
