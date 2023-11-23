using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.FlashSale;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFlashSaleRepository : IRepository<FlashSale, int>
    {
        PagedList<Food> ListFoodAvailable(CheckFoodFlashSaleParameters parameters);
		PagedList<FlashSale> ListFlashSaleByStore(int storeId, FlashSaleParameter parameter);
		Task DeleteFlashSale(int flashSaleId);
		Task DeleteFlashSaleDetail(int flashSaleId, int foodId);

    }
}
