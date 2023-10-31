using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
    public class OrderDetailRepository : EntityRepository<OrderDetail, int>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
