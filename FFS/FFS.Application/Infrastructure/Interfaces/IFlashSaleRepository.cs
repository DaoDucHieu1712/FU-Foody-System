using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFlashSaleRepository : IRepository<FlashSale, int>
    {
        PagedList<Food> ListFoodAvailable(CheckFoodFlashSaleParameters parameters);
        Task DeleteFlashSale(int flashSaleId);

    }
}
