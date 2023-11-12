using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<List<Category>> Top5PopularCategories();
    }
}
