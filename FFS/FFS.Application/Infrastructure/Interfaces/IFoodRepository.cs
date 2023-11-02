using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFoodRepository : IRepository<Food, int>
    {
        Task<List<Food>> GetFoodListByStoreId(int storeId);
        PagedList<Food> GetFoods(FoodParameters foodParameters);
    }
}
