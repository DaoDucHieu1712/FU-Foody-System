﻿using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IFoodRepository : IRepository<Food, int>
    {
        Task<List<Food>> GetFoodListByStoreId(int storeId);
    }
}
