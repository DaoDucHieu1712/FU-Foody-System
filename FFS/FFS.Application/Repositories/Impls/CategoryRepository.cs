using AutoMapper;
using ClosedXML.Excel;
using FFS.Application.Data;
using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class CategoryRepository : EntityRepository<Category, int>, ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<byte[]> ExportCategory(int id)
        {
            try
            {
                List<Category> categories = await _context.Categories
                    .Where(x => x.StoreId == id && x.IsDelete == false)
                    .ToListAsync();

                var exportCategories = _mapper.Map<List<CategoryDTO>>(categories);

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.ExportCategory(exportCategories, workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return await Task.FromResult(stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DTOs.Common.PagedList<Category> GetCategoriesByStoreId(CategoryParameters categoryParameters)
        {
            var query = FindAll(i => i.StoreId == categoryParameters.StoreId);

            // Filter by food name if specified
            if (!string.IsNullOrEmpty(categoryParameters.CategoryName))
            {
                var categoryNameLower = categoryParameters.CategoryName.ToLower();

                query = query.ToList()
                    .Where(i => CommonService.RemoveDiacritics(i.CategoryName.ToLower()).Contains(CommonService.RemoveDiacritics(categoryNameLower))).AsQueryable();
            }

            // Apply pagination
            var pagedList = DTOs.Common.PagedList<Category>.ToPagedList(
                query.Include(s => s.Store),
                categoryParameters.PageNumber,
                categoryParameters.PageSize
            );

            return pagedList;
        }

        public async Task<List<Category>> Top8PopularCategories()
        {
            try
            {
                var popularCategories = await _context.Categories
                    .Include(x => x.Foods)
                        .ThenInclude(x => x.OrderDetails)
                    .OrderByDescending(x => x.Foods.SelectMany(food => food.OrderDetails).Sum(od => od.Quantity)).ToListAsync();
                if (popularCategories.Count() < 8)
                {
                    popularCategories = popularCategories.DistinctBy(x => x.CategoryName).ToList();
                }
                else
                {
                    popularCategories = popularCategories.DistinctBy(x=>x.CategoryName).Take(10).ToList(); 
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
