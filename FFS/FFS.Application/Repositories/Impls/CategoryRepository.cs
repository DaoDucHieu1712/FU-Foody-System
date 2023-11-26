using System.Data;

﻿using System.Drawing;
using System.IO.Packaging;
using AutoMapper;
using ClosedXML.Excel;

using Dapper;

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
				var parameters = new DynamicParameters();
				parameters.Add("storeId", id);
				using var db = _context.Database.GetDbConnection();

				dynamic exportFoods = db.Query<dynamic>("ExportCategory", parameters, commandType: CommandType.StoredProcedure);
				db.Close();

				using (var package = new ExcelPackage())
				{
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("Foods");


					int index = 1;
					string cell = string.Format($"A{index}:E{index}");
					worksheet.Cells[cell].Value = "Báo cáo doanh mục thực phẩm";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(System.Drawing.ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Tên danh mục";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Tên cửa hàng";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Số lượng sản phẩm";

					cell = string.Format($"D{index}");
					worksheet.Cells[cell].Value = "Ngày tạo";

					cell = string.Format($"E{index}");
					worksheet.Cells[cell].Value = "Lần cập nhật cuối cùng";


					cell = string.Format($"A{index}:E{index}");
					worksheet.Cells[cell].Style.Font.Size = 14;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

					index++;


					int indexData = 0;

					// Populate rows with data
					for (int i = 0; i < exportFoods.Count; i++)
					{
						indexData = index + i;
						cell = string.Format($"A{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].CategoryName;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].StoreName;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].totalFoods;

						cell = string.Format($"D{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].CreatedAt;

						cell = string.Format($"E{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].UpdatedAt;

						worksheet.Row(indexData).Height = 30;
				
					}

					cell = string.Format($"A{2}:E{indexData}");
					worksheet.Cells[cell].Style.Font.Size = 14;

					worksheet.Cells.AutoFitColumns();

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
