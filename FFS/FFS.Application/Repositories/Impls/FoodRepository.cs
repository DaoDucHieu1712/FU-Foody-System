using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories.Impls
{
    public class FoodRepository : EntityRepository<Food, int>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext _dbContext) : base(_dbContext) { }

    }
}
