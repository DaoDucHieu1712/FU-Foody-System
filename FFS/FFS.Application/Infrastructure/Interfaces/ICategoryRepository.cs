using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<List<Category>> Top5PopularCategories();
        PagedList<Category> GetCategoriesByStoreId(CategoryParameters categoryParameters);
    }
}
