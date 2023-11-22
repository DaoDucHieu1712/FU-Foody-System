using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFlashSaleDetailRepository 
    {
        Task CreateFlashSaleDetail(FlashSaleDetail detail);
        Task<FlashSaleDetail> GetFlashSaleDetail(int foodId, int flashSaleId);
    }
}
