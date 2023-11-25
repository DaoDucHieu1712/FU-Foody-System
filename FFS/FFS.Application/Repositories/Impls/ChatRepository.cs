using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
	public class ChatRepository : EntityRepository<Chat, int>, IChatRepository
	{
		public ChatRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
