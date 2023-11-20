using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class FlashSaleDetailRepository : EntityRepository<FlashSaleDetail, int>, IFlashSaleDetailRepository
    {
        public FlashSaleDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
