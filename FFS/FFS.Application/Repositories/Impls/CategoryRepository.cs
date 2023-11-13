using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class CategoryRepository : EntityRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext _dbContext) : base(_dbContext) { }

        public PagedList<Category> GetCategoriesByStoreId(CategoryParameters categoryParameters)
        {
            var query = FindAll(i => i.StoreId == categoryParameters.StoreId);

            // Filter by food name if specified
            if (!string.IsNullOrEmpty(categoryParameters.CategoryName))
            {
                var categoryNameLower = categoryParameters.CategoryName.ToLower();

                query = query
                    .Where(i => i.CategoryName.ToLower().Contains(categoryNameLower));
            }

            // Apply pagination
            var pagedList = PagedList<Category>.ToPagedList(
                query.Include(s => s.Store),
                categoryParameters.PageNumber,
                categoryParameters.PageSize
            );

            return pagedList;
        }

        public async Task<List<Category>> Top5PopularCategories()
        {
            try
            {
                var popularCategories = await _context.Categories
                    .Include(x => x.Foods)
                        .ThenInclude(x => x.OrderDetails)
                    .OrderByDescending(x => x.Foods.SelectMany(food => food.OrderDetails).Sum(od => od.Quantity)).ToListAsync();
                if (popularCategories.Count() < 5)
                {
                    popularCategories = popularCategories.ToList();
                }
                else
                {
                    popularCategories = popularCategories.Take(5).ToList(); 
                }

                return popularCategories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
