using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
    public class OrderRepository : EntityRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
