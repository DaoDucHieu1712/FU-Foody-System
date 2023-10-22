using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class InventoryRepository : EntityRepository<Inventory, int>, IInventoryRepository
    {
        public InventoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task CreateInventory(Inventory inventory)
        {
            await Add(inventory);
        }

        //public async Task UpdateInventory(Inventory inventory)
        //{
        //    await Update(inventory);
        //}
        public async Task UpdateInventoryByStoreAndFoodId(int storeId, int foodId, int newQuantity)
        {
            var inventory = await FindSingle(i => i.StoreId == storeId && i.FoodId == foodId);

            if (inventory != null)
            {
                inventory.quantity = newQuantity;
                await Update(inventory);
            }
        }
        
        public PagedList<Inventory> GetInventories(InventoryParameters inventoryParameters)
        {
            var query = FindAll(i => i.StoreId == inventoryParameters.StoreId);

            // Filter by food name if specified
            if (!string.IsNullOrEmpty(inventoryParameters.FoodName))
            {
                var foodNameLower = inventoryParameters.FoodName.ToLower();

                query = query
                    .Where(i => i.Food.FoodName.ToLower().Contains(foodNameLower));
            }

            // Apply pagination
            var pagedList = PagedList<Inventory>.ToPagedList(
                query.Include(f=> f.Food).ThenInclude(s=>s.Store),
                inventoryParameters.PageNumber,
                inventoryParameters.PageSize
            );

            return pagedList;
        }

        public async Task DeleteInventoryByFoodId(int foodId)
        {
            var inventory = await FindSingle(i => i.FoodId == foodId);
            if (inventory != null)
            {
                await Remove(inventory);
            }
        }
    }
}
