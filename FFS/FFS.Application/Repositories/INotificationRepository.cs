using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
	public interface INotificationRepository : IRepository<Notification, int>
	{
		Task<List<Notification>> GetNotificationsByUserId(string userId);


	}
	
}
