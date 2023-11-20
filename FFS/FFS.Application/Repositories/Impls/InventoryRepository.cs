using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Helper;
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
            var query = FindAll(i => i.StoreId == inventoryParameters.StoreId).Include(x=>x.Food).AsQueryable();
         
            // Filter by food name if specified
            if (!string.IsNullOrEmpty(inventoryParameters.FoodName))
            {
                var foodNameLower = inventoryParameters.FoodName.ToLower();

                query = query.ToList()
                    .Where(i => CommonService.RemoveDiacritics(i.Food.FoodName.ToLower()).Contains(CommonService.RemoveDiacritics(foodNameLower))).AsQueryable();
            }

            // Apply pagination
            var pagedList = PagedList<Inventory>.ToPagedList(
                query.Include(f=> f.Food).ThenInclude(c=>c.Category).ThenInclude(s=>s.Store),
                inventoryParameters.PageNumber,
                inventoryParameters.PageSize
            );

            return pagedList;
        }

        public async Task DeleteInventoryByInventoryId(int inventoryId)
        {
            var inventory = await FindSingle(i => i.Id == inventoryId);
            if (inventory != null)
            {
                await Remove(inventory);
            }
            else
            {
                throw new Exception("Không tìm thấy kho này!");
            }
        }

        public async Task<Inventory> GetInventoryByFoodAndStore(int storeId, int foodId)
        {
            return await FindSingle(i => i.StoreId == storeId && i.FoodId == foodId);
        }

    }
}
