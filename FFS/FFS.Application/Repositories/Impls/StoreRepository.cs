using System.Data;
using System.Drawing;
using AutoMapper;
using Dapper;

using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Location;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Helper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace FFS.Application.Repositories.Impls
{
	public class StoreRepository : EntityRepository<Store, int>, IStoreRepository
	{

		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public StoreRepository(ApplicationDbContext context, IMapper mapper) : base(context)
		{
			_context = context;
			_mapper = mapper;
		}


		public async Task<byte[]> ExportFood(int id)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("storeId", id);
				using var db = _context.Database.GetDbConnection();

				dynamic exportFoods = db.Query<dynamic>("ExportFood", parameters, commandType: CommandType.StoredProcedure);
				db.Close();

				using (var package = new ExcelPackage())
				{
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("Foods");


					int index = 1;
					string cell = string.Format($"A{index}:G{index}");
					worksheet.Cells[cell].Value = "Báo cáo thực phẩm";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(System.Drawing.ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Tên món";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Đường dẫn ảnh";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Mô tả";

					cell = string.Format($"D{index}");
					worksheet.Cells[cell].Value = "Giá/món";

					cell = string.Format($"E{index}");
					worksheet.Cells[cell].Value = "Loại đồ ăn";

					cell = string.Format($"F{index}");
					worksheet.Cells[cell].Value = "Điểm trung bình";

					cell = string.Format($"G{index}");
					worksheet.Cells[cell].Value = "Lượt đánh giá";



					cell = string.Format($"A{index}:G{index}");
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
						worksheet.Cells[cell].Value = exportFoods[i].FoodName;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].ImageURL;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].Description;

						cell = string.Format($"D{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].Price;

						cell = string.Format($"E{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].CategoryName;

						cell = string.Format($"F{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].RateAverage;

						cell = string.Format($"G{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].TotalRate;

						worksheet.Row(indexData).Height = 30;

						if (exportFoods[i].IsDelete == true)
						{
							worksheet.Row(indexData).Style.Font.Color.SetColor(Color.Red);
						}
					}


					cell = string.Format($"A{2}:G{indexData}");
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

		public async Task<byte[]> ExportInventory(int id)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("storeId", id);
				using var db = _context.Database.GetDbConnection();

				dynamic exportFoods = db.Query<dynamic>("ExportInventory", parameters, commandType: CommandType.StoredProcedure);
				db.Close();

				using (var package = new ExcelPackage())
				{
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("Foods");


					int index = 1;
					string cell = string.Format($"A{index}:H{index}");
					worksheet.Cells[cell].Value = "Báo cáo tồn kho sản phẩm";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(System.Drawing.ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Tên món";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Đường dẫn ảnh";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Mô tả";

					cell = string.Format($"D{index}");
					worksheet.Cells[cell].Value = "Giá/món";

					cell = string.Format($"E{index}");
					worksheet.Cells[cell].Value = "Loại đồ ăn";

					cell = string.Format($"F{index}");
					worksheet.Cells[cell].Value = "Điểm trung bình";

					cell = string.Format($"G{index}");
					worksheet.Cells[cell].Value = "Lượt đánh giá";

					cell = string.Format($"H{index}");
					worksheet.Cells[cell].Value = "Số lượng tồn kho";


					cell = string.Format($"A{index}:H{index}");
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
						worksheet.Cells[cell].Value = exportFoods[i].FoodName;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].ImageURL;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].Description;

						cell = string.Format($"D{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].Price;

						cell = string.Format($"E{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].CategoryName;

						cell = string.Format($"F{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].RateAverage;

						cell = string.Format($"G{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].TotalRate;

						cell = string.Format($"H{indexData}");
						worksheet.Cells[cell].Value = exportFoods[i].quantity;

						worksheet.Row(indexData).Height = 30;

						if (exportFoods[i].IsDelete == true)
						{
							worksheet.Row(indexData).Style.Font.Color.SetColor(Color.Red);
						}
					}


					cell = string.Format($"A{2}:H{indexData}");
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

		public async Task<StoreInforDTO> GetDetailStore(int id)
		{
			try
			{
				Store? stores = await FindById(id, x => x.Foods, x => x.FoodCombos, x => x.Discounts, x => x.Categories);
				if (stores == null)
				{
					throw new Exception("Cửa hàng không tồn tại!");
				}
				else
				{
					StoreInforDTO storeInforDTO = _mapper.Map<StoreInforDTO>(stores);
					return storeInforDTO;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<dynamic> GetCommentByStore(int rate, int id)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				parameters.Add("storeId", id);
				parameters.Add("rate", rate);


				using (var db = _context.Database.GetDbConnection())
				{
					if (db.State != ConnectionState.Open)
					{
						db.Open();
					}

					returnData = await db.QueryAsync<dynamic>("GetCommentByStore", parameters, commandType: CommandType.StoredProcedure);
				}
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<List<FoodDTO>> GetFoodByCategory(int idShop, int idCategory)
		{
			try
			{
				List<Food> foods = await _context.Foods.Where(x => x.StoreId == idShop && x.CategoryId == idCategory && x.IsDelete==false).Include(x => x.Inventories).Include(x => x.FlashSaleDetails).ThenInclude(x => x.FlashSale).ToListAsync();
				if (foods.Count == 0)
				{
					throw new Exception();
				}
				else
				{
					List<FoodDTO> foodDTOs = _mapper.Map<List<FoodDTO>>(foods);
					return foodDTOs;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<StoreInforDTO> GetInformationStore(int id)
		{
			try
			{
				Store? stores = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
				if (stores == null)
				{
					throw new Exception("Cửa hàng không tồn tại!");
				}
				else
				{
					StoreInforDTO storeInforDTO = _mapper.Map<StoreInforDTO>(stores);
					return storeInforDTO;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<StoreInforDTO> UpdateStore(int id, StoreInforDTO storeInforDTO)
		{
			try
			{
				Store? store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
				if (store == null)
				{
					throw new Exception("Cửa hàng không tồn tại!");
				}
				else
				{
					var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == storeInforDTO.UserId);
					user.Avatar = storeInforDTO.AvatarURL;
					user.UserName = storeInforDTO.StoreName;
					await _context.SaveChangesAsync();

					store.StoreName = storeInforDTO.StoreName;
					if (storeInforDTO.AvatarURL != null)
					{
						store.AvatarURL = storeInforDTO.AvatarURL;
					}
					store.PhoneNumber = storeInforDTO.PhoneNumber;
					store.Description = storeInforDTO.Description;
					store.TimeStart = storeInforDTO.TimeStart;
					store.TimeEnd = storeInforDTO.TimeEnd;

					Location location = await _context.Locations.FirstOrDefaultAsync(x => x.UserId == store.UserId);
					if(location != null)
					{
						LocationDTO locationDTO = storeInforDTO.Location;

						location.DistrictID = locationDTO.DistrictID;
						location.DistrictName = locationDTO.DistrictName;
						location.ProvinceID = locationDTO.ProvinceID;
						location.ProvinceName = locationDTO.ProvinceName;
						location.WardCode = locationDTO.WardCode;
						location.WardName = locationDTO.WardName;
						location.Address = locationDTO.Address;
						location.Description = locationDTO.Description;
						location.PhoneNumber = locationDTO.PhoneNumber;

						store.Address = $"{locationDTO.Address}, {locationDTO.WardName}, {locationDTO.DistrictName}, {locationDTO.ProvinceName}";
					}



					_ = await _context.SaveChangesAsync();
					return storeInforDTO;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<dynamic> GetCommentReply(int id)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				parameters.Add("commentId", id);


				using (var db = _context.Database.GetDbConnection())
				{
					if (db.State != ConnectionState.Open)
					{
						db.Open();
					}

					returnData = await db.QueryAsync<dynamic>("GetReplyComment", parameters, commandType: CommandType.StoredProcedure);
				}
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<List<Store>> GetTop10PopularStore()
		{
			try
			{
				var popularStores = await _context.OrderDetails
					.Include(od => od.Store)
					.GroupBy(od => od.StoreId)
					.Select(group => new
					{
						StoreId = group.Key,
						OrderCount = group.Count()
					})
					.OrderByDescending(x => x.OrderCount)
					.Take(10)
					.ToListAsync();

				var storeIds = popularStores.Select(ps => ps.StoreId).ToList();

				var topStores = await _context.Stores
					.Where(s => storeIds.Contains(s.Id))
					.ToListAsync();

				return topStores;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public PagedList<Store> GetAllStores(AllStoreParameters allStoreParameters)
		{
			var query = _context.Stores.Include(x => x.Categories).AsQueryable();

			if (!string.IsNullOrEmpty(allStoreParameters.CategoryName)  && !allStoreParameters.CategoryName.Equals("Tất cả"))
			{
				query = query.Where(store => store.Categories.Any(category => category.Id == Convert.ToInt32(allStoreParameters.CategoryName)));
			}
			SearchByStoreName(ref query, allStoreParameters.Search);
			if (!string.IsNullOrEmpty(allStoreParameters.FilterStore.ToString()))
			{
				switch (allStoreParameters.FilterStore)
				{
					case FilterStore.StoreNameAcs:
						FilterByStoreNameAcs(ref query);
						break;
					case FilterStore.StoreNameDesc:
						FilterByStoreNameDesc(ref query);
						break;
					case FilterStore.TopRated:
						FilterByTopRated(ref query);
						break;
					default:
						break;
				}
			}

			//Apply pagination
			var pagedList = PagedList<Store>.ToPagedList(
				query,
				allStoreParameters.PageNumber,
				allStoreParameters.PageSize
			);

			return pagedList;
		}
		private void SearchByStoreName(ref IQueryable<Store> stores, string search)
		{
			if (!stores.Any() || string.IsNullOrWhiteSpace(search))
				return;

			var allStores = stores.ToList();

			// Loại bỏ dấu từ chuỗi tìm kiếm
			string searchWithoutDiacritics = CommonService.RemoveDiacritics(search);

			// Thực hiện tìm kiếm không phân biệt dấu và không phân biệt chữ hoa/chữ thường
			stores = allStores
				.Where(o => CommonService.RemoveDiacritics(o.StoreName).ToLower().Contains(searchWithoutDiacritics.ToLower()))
				.AsQueryable();
		}

		private void FilterByStoreNameAcs(ref IQueryable<Store> stores)
		{
			if (!stores.Any())
				return;
			stores = stores.OrderBy(x => x.StoreName);
			;
		}
		private void FilterByStoreNameDesc(ref IQueryable<Store> stores)
		{
			if (!stores.Any())
				return;
			stores = stores.OrderByDescending(x => x.StoreName);
			;
		}
		private void FilterByTopRated(ref IQueryable<Store> stores)
		{
			if (!stores.Any())
				return;
			stores = stores.OrderByDescending(x => x.RateAverage);
			;
		}

	}
}
