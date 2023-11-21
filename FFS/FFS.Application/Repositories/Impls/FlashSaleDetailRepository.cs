using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class FlashSaleDetailRepository : IFlashSaleDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public FlashSaleDetailRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task CreateFlashSaleDetail(FlashSaleDetail detail)
        {
            await _context.FlashSaleDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

      

        public async Task<FlashSaleDetail> GetFlashSaleDetail(int foodId, int flashSaleId)
        {
            return await _context.FlashSaleDetails.FirstOrDefaultAsync(x => x.FlashSaleId == flashSaleId && x.FoodId == foodId);
        }
    }
}
