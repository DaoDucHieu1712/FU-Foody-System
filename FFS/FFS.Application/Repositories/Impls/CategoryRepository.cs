using System.Drawing;
using System.IO.Packaging;
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
using OfficeOpenXml;

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

                using (var package = new ExcelPackage())
                {
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("Danh mục");
					int index = 1;
					string cell = string.Format($"A{index}:C{index}");
					worksheet.Cells[cell].Value = "Báo cáo Danh mục";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Mã danh mục";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Tên danh mục";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Ngày tạo";

					cell = string.Format($"A{index}:C{index}");
					worksheet.Cells[cell].Style.Font.Size = 14;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

					index++;


					int indexData = 0;

					// Populate rows with data
					for (int i = 0; i < exportCategories.Count; i++)
					{
						indexData = index + i;

						cell = string.Format($"A{indexData}");
						worksheet.Cells[cell].Value = exportCategories[i].Id;
						worksheet.Cells[cell].Style.WrapText = true;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportCategories[i].CategoryName;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportCategories[i].CreatedAt.ToString("MM/dd/yyyy HH:mm:ss");


						worksheet.Row(indexData).Height = 40;
					}


					cell = string.Format($"A{2}:C{indexData}");
					worksheet.Cells[cell].Style.Font.Size = 14;
					worksheet.Cells.AutoFitColumns();
					worksheet.Column(1).Width = 85;


					return package.GetAsByteArray();
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
