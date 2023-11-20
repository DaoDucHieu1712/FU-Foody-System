using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class FlashSaleRepository : EntityRepository<FlashSale, int>, IFlashSaleRepository
    {
        public FlashSaleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
