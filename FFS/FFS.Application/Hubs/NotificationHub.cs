using FFS.Application.Entities;
using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Hubs
{
    public class NotificationHub: Hub
    {
		//public async Task SendNotification(Notification notification)
		//{
		//	await Clients.All.SendAsync("ReceiveNotification", notification);
		//}

		//public async Task SendNotificationToUser(string userId, Notification notification)
		//{
		//	await Clients.Group(userId).SendAsync("ReceiveNotification", notification);
		//}

		//public async Task SendNotificationToStore(string storeId, Notification notification)
		//{
		//	await Clients.Group(storeId).SendAsync("ReceiveNotification", notification);
		//}

		


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
