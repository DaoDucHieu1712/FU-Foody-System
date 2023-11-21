using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.FlashSale;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class FlashSaleRepository : EntityRepository<FlashSale, int>, IFlashSaleRepository
    {
        public FlashSaleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task DeleteFlashSale(int flashSaleId)
        {
            try
            {
                var flashSaleDetailsToDelete = await _context.FlashSaleDetails
                    .Where(detail => detail.FlashSaleId == flashSaleId)
                    .ToListAsync();

                _context.FlashSaleDetails.RemoveRange(flashSaleDetailsToDelete);

                var flashSaleToDelete = await _context.FlashSales
                    .FirstOrDefaultAsync(x => x.Id == flashSaleId);

                if (flashSaleToDelete != null)
                {
                    _context.FlashSales.Remove(flashSaleToDelete);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting FlashSale and associated details for FlashSaleId {flashSaleId}: {ex.Message}", ex);
            }
        }

        public PagedList<Food> ListFoodAvailable(CheckFoodFlashSaleParameters parameters)
        {
            var listFS = _context.FlashSales.Include(x => x.FlashSaleDetails).FirstOrDefault(x => x.StoreId == parameters.StoreId && x.Start <= parameters.Start && x.End >= parameters.End);
            List<int> foodIds = new List<int>();
            if (listFS != null)
            {
                foodIds = listFS.FlashSaleDetails.Select(x => x.FoodId).ToList();
            }
            var foodsNotInFlashSale = _context.Foods
     .Include(x => x.Inventories)
     .Include(x => x.Category)
     .Where(x => x.StoreId == parameters.StoreId && !foodIds.Contains(x.Id))
     .AsQueryable();

            // Apply pagination
            var pagedList = PagedList<Food>.ToPagedList(
                foodsNotInFlashSale,
                parameters.PageNumber,
                parameters.PageSize
            );

            return pagedList;
        }
    }
}
