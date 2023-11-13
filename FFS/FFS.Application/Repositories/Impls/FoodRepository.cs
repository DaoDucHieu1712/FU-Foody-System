using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Data;

using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;
using FFS.Application.Entities.Enum;

namespace FFS.Application.Repositories.Impls
{
    public class FoodRepository : EntityRepository<Food, int>, IFoodRepository
    {
        private readonly DapperContext _dapperContext;
        public FoodRepository(ApplicationDbContext _dbContext, DapperContext dapperContext) : base(_dbContext) {
            _dapperContext = dapperContext;
        }
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
        public dynamic GetFoods(FoodParameters foodParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                parameters.Add("userId", foodParameters.uId);
                parameters.Add("foodName", foodParameters.FoodName);
                parameters.Add("pageNumber", foodParameters.PageNumber);
                parameters.Add("pageSize", foodParameters.PageSize);
                using (var db = _dapperContext.connection)
                {

                    returnData = db.Query<dynamic>("GetFoodsByStore", parameters, commandType: CommandType.StoredProcedure);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int CountGetFoods(FoodParameters foodParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                parameters.Add("userId", foodParameters.uId);
                parameters.Add("foodName", foodParameters.FoodName);

                using (var db = _dapperContext.connection)
                {
                   
                    returnData = db.QuerySingle<int>("CountGetFoodsByStore", parameters, commandType: CommandType.StoredProcedure);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public PagedList<Food> GetAllFoods(AllFoodParameters allFoodParameters)
        {
            var query = _context.Foods.Include(x => x.Category).Include(x => x.OrderDetails).AsQueryable();

            if (!string.IsNullOrEmpty(allFoodParameters.CatId.ToString()))
            {
                query = query.Where(f => f.CategoryId == allFoodParameters.CatId);
            }
            SearchByProductAndCategoryName(ref query, allFoodParameters.Search);
            SearchByPriceRange(ref query, allFoodParameters.PriceMin, allFoodParameters.PriceMax);
            if (!string.IsNullOrEmpty(allFoodParameters.FilterFood.ToString()))
            {
                switch (allFoodParameters.FilterFood)
                {
                    case FilterFood.Flashsale:
                        break;
                    case FilterFood.Bestseller:
                        FilterByBestSeller(ref query);
                        break;
                    case FilterFood.TopRated:
                        FilterByTopRated(ref query);
                        break;
                    case FilterFood.LatestProduct:
                        FilterByLatestProduct(ref query);
                        break;
                    default:
                        break;
                }
            }

            //Apply pagination
            var pagedList = PagedList<Food>.ToPagedList(
                query,
                allFoodParameters.PageNumber,
                allFoodParameters.PageSize
            );

            return pagedList;
        }
        private void SearchByProductAndCategoryName(ref IQueryable<Food> foods, string search)
        {
            if (!foods.Any() || string.IsNullOrWhiteSpace(search))
                return;
            foods = foods.Where(o => o.Category.CategoryName.ToLower().Contains(search.Trim().ToLower()) || o.FoodName.ToLower().Contains(search.Trim().ToLower()));
        }
        private void SearchByPriceRange(ref IQueryable<Food> foods, decimal? minPrice, decimal? maxPrice)
        {
            if (!foods.Any())
                return;
            foods = foods
                .Where(o => o.Price >= minPrice && o.Price <= maxPrice);
        }
        private void FilterByTopRated(ref IQueryable<Food> foods)
        {
            if (!foods.Any())
                return;
            foods = foods.OrderByDescending(x => x.RateAverage);
            ;
        }
        private void FilterByLatestProduct(ref IQueryable<Food> foods)
        {
            if (!foods.Any())
                return;
            foods = foods.OrderByDescending(x => x.CreatedAt);
            ;
        }
        private void FilterByBestSeller(ref IQueryable<Food> foods)
        {
            if (!foods.Any())
                return;
            foods = foods
         .OrderByDescending(x => x.OrderDetails.Sum(od => od.Quantity));
            ;
        }

        public async Task<Food> GetFoodById(int id)
        {
            try
            {
                var food =  await _context.Foods.Include(x => x.Category).Include(x => x.Inventories).Include
                (x => x.Store).Include(x=>x.Comments).FirstOrDefaultAsync(x => x.Id == id);
                return food;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
