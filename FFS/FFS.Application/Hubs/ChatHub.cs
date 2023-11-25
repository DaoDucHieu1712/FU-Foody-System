using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Hubs
{
	public sealed class ChatHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("Receive Message", $"{Context.ConnectionId} has joined");
		}

		public async Task SendMessage(string msg)
		{
			await Clients.All.SendAsync("ReceiverMessage", $"{Context.ConnectionId}: {msg}");
		}

	}
}
