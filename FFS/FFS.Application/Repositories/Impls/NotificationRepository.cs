using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
	public class NotificationRepository : EntityRepository<Notification, int>, INotificationRepository
	{
		public NotificationRepository(ApplicationDbContext context) : base(context)
		{
		}
		public async Task<List<Notification>> GetNotificationsByUserId(string userId)
		{
			try
			{
				return await FindAll(n => n.UserId == userId).ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		
	}
}
