using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Repositories
{
    public interface IInventoryRepository
    {
        Task CreateInventory(Inventory inventory);
        Task UpdateInventoryByStoreAndFoodId(int storeId, int foodId, int newQuantity);
        PagedList<Inventory> GetInventories(InventoryParameters inventoryParameters);
        Task DeleteInventoryByFoodId(int foodId);
    }
}
