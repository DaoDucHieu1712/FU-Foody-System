using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory,int>
    {
        Task CreateInventory(Inventory inventory);
        Task UpdateInventoryByStoreAndFoodId(int storeId, int foodId);
        Task ExportInventory(int storeId, int foodId, int quantity);
        Task ImportInventory(int storeId, int foodId, int quantity);
        PagedList<Inventory> GetInventories(InventoryParameters inventoryParameters);
        Task DeleteInventoryByInventoryId(int inventoryId);
        Task<Inventory> GetInventoryByFoodAndStore(int storeId, int foodId);
    }
}
