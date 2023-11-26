using System.Data;
using System.Drawing;
using Dapper;
using FFS.Application.Data;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace FFS.Application.Repositories.Impls
{
	public class UserRepository : IUserRepository
	{

		private readonly ApplicationDbContext _context;
		private readonly DapperContext _dapperContext;


		public UserRepository(ApplicationDbContext context, DapperContext dapperContext)
		{
			_context = context;
			_dapperContext = dapperContext;
		}



		public int CountGetUsers(UserParameters userParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("username", userParameters.Username);
				parameters.Add("email", userParameters.Email);
				parameters.Add("role", userParameters.Role);


				using var db = _dapperContext.connection;

				returnData = db.QuerySingle<int>("CountGetUsers", parameters, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<byte[]> ExportUser()
		{
			try
			{
				using var db = _context.Database.GetDbConnection();

				dynamic exportReports = db.Query<dynamic>("ExportUser", commandType: CommandType.StoredProcedure);
				db.Close();

				using (var package = new ExcelPackage())
				{
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("User");


					int index = 1;
					string cell = string.Format($"A{index}:G{index}");
					worksheet.Cells[cell].Value = "Báo cáo tài khoản người dùng";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Avatar";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Username";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Email";

					cell = string.Format($"D{index}");
					worksheet.Cells[cell].Value = "Số điện thoại";

					cell = string.Format($"E{index}");
					worksheet.Cells[cell].Value = "Role";

					cell = string.Format($"F{index}");
					worksheet.Cells[cell].Value = "Trạng thái";

					cell = string.Format($"G{index}");
					worksheet.Cells[cell].Value = "Trạng thái duyệt";



					cell = string.Format($"A{index}:G{index}");
					worksheet.Cells[cell].Style.Font.Size = 14;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

					index++;


					int indexData = 0;

					// Populate rows with data
					for (int i = 0; i < exportReports.Count; i++)
					{
						indexData = index + i;

						cell = string.Format($"A{indexData}");
						worksheet.Cells[cell].Hyperlink = new Uri(exportReports[i].Avatar);
						worksheet.Cells[cell].Style.WrapText = true;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].Email;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].UserName;

						cell = string.Format($"D{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].PhoneNumber;

						cell = string.Format($"E{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].Role;

						cell = string.Format($"F{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].Allow;

						cell = string.Format($"G{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].Status;

						worksheet.Row(indexData).Height = 40;
						if (exportReports[i].AllowCode == false)
						{
							worksheet.Row(indexData).Style.Font.Color.SetColor(Color.Red);
						}

					}


					cell = string.Format($"A{2}:G{indexData}");
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

		public IEnumerable<dynamic> GetRequestCreateAccount(UserParameters userParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("username", userParameters.Username);
				parameters.Add("email", userParameters.Email);
				parameters.Add("role", userParameters.Role);
				parameters.Add("status", userParameters.Status);
				parameters.Add("pageNumber", userParameters.PageNumber);
				parameters.Add("pageSize", userParameters.PageSize);

				using var db = _dapperContext.connection;

				returnData = db.Query<dynamic>("GetRequestCreateAccount", parameters, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public int CountGetRequestCreateAccount(UserParameters userParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("username", userParameters.Username);
				parameters.Add("email", userParameters.Email);
				parameters.Add("role", userParameters.Role);
				parameters.Add("status", userParameters.Status);



				using var db = _dapperContext.connection;

				returnData = db.QuerySingle<int>("CountGetRequestCreateAccount", parameters, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		public IEnumerable<dynamic> GetRoles()
		{
			return _context.Roles.ToList();
		}

		public IEnumerable<dynamic> GetUsers(UserParameters userParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("username", userParameters.Username);
				parameters.Add("email", userParameters.Email);
				parameters.Add("role", userParameters.Role);
				parameters.Add("pageNumber", userParameters.PageNumber);
				parameters.Add("pageSize", userParameters.PageSize);

				using var db = _dapperContext.connection;

				returnData = db.Query<dynamic>("GetUsers", parameters, commandType: CommandType.StoredProcedure);
				db.Close();

				List<Combo> c = _context.Combos.ToList();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public void BanAccount(string idBan)
		{
			try
			{
				var user = _context.Users.FirstOrDefault(x => x.Id == idBan);
				if (user is null)
				{
					throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
				}
				user.Allow = false;
				_ = _context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public void UnBanAccount(string idUnBan)
		{
			try
			{
				var user = _context.Users.FirstOrDefault(x => x.Id == idUnBan);
				if (user is null)
				{
					throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
				}
				user.Allow = true;
				_ = _context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public void ApproveUser(string id, string action)
		{
			try
			{
				var user = _context.Users.FirstOrDefault(x => x.Id == id);
				if (user is null)
				{
					throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
				}
				if (action == "Accept")
				{
					user.Status = StatusUser.Accept;
				}
				if (action == "Reject")
				{
					user.Status = StatusUser.Reject;
				}
				_ = _context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public List<AccountStatistic> AccountsStatistic()
		{
			try
			{
				var accountStatistic = _context.ApplicationUsers.GroupBy(user => user.Status).Select(group => new AccountStatistic
				{
					UserType = group.Key,
					NumberOfAccount = group.Count()
				})
	.ToList();
				return accountStatistic;
			} catch (Exception ex)
			{
				throw new Exception(ex.Message);	
			}
		}

		public int CountTotalUsers()
		{
			return _context.ApplicationUsers.Count();
		}
	}
}
