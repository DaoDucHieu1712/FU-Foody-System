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

                using (var db = _context.Database.GetDbConnection())
                {
                    if (db.State != ConnectionState.Open)
                    {
                        db.Open();
                    }

                    returnData = db.Query<dynamic>("GetFoodsByStore", parameters, commandType: CommandType.StoredProcedure);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

     
    }
}
