using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
	public class UserDiscountRepository : EntityRepository<UserDiscount, int>, IUserDiscountRepository
	{
		public UserDiscountRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
