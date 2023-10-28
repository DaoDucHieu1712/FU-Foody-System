using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class FoodRepository : EntityRepository<Food, int>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext _dbContext) : base(_dbContext) { }
        public async Task<List<Food>> GetFoodListByStoreId(int storeId)
        {
            try
            {
                // Filter the food items by the provided StoreId
                var foodList = await GetList(f => f.StoreId == storeId);
                return foodList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
