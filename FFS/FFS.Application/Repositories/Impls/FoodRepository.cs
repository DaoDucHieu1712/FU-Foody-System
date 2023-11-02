using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
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
        public PagedList<Food> GetFoods(FoodParameters foodParameters)
        {
            var query = FindAll(x => x.StoreId == foodParameters.StoreId && x.IsDelete == false, x => x.Category);

            if (!string.IsNullOrEmpty(foodParameters.FoodName))
            {
                var foodNameLower = foodParameters.FoodName.ToLower();

                query = query
                    .Where(i => i.FoodName.ToLower().Contains(foodNameLower));
            }
            // Apply pagination
            var pagedList = PagedList<Food>.ToPagedList(
                query.Include(f => f.Category),
                foodParameters.PageNumber,
                foodParameters.PageSize
            );

            return pagedList;
        }
    }
}
