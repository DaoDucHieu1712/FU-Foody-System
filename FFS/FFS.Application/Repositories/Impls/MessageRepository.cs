using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
	public class MessageRepository : EntityRepository<Message, int>, IMessageRepository
	{
		public MessageRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
