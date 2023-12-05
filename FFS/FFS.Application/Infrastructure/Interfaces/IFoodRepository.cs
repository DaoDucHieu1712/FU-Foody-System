using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFoodRepository : IRepository<Food, int>
    {
        Task<List<Food>> GetFoodListByStoreId(int storeId);
        dynamic GetFoods(FoodParameters foodParameters);
        int CountGetFoods(FoodParameters foodParameters);

        PagedList<Food> GetAllFoods(AllFoodParameters allFoodParameters);

        Task<Food> GetFoodById(int id);
	}
}
