using System.Data;
using Dapper;
using DocumentFormat.OpenXml.InkML;
using FFS.Application.Data;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace FFS.Application.Repositories.Impls
{
	public class ReportRepository : EntityRepository<Report, int>, IReportRepository
	{
		public ReportRepository(ApplicationDbContext context) : base(context)
		{
		}

		public int CountGetReports(ReportParameters reportParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("usernameReport", reportParameters.UsernameReport);
				parameters.Add("description", reportParameters.Description);

				using var db = _context.Database.GetDbConnection();

				returnData = db.QuerySingle<int>("CountGetReports", parameters, commandType: CommandType.StoredProcedure);

				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task CreateReport(Report report)
		{
			await Add(report);
		}

		public async Task<byte[]> ExportReport()
		{
			try
			{
				var parameters = new DynamicParameters();
				using var db = _context.Database.GetDbConnection();

				dynamic exportReports = db.Query<dynamic>("ExportReport", parameters, commandType: CommandType.StoredProcedure);
				db.Close();

				using (var package = new ExcelPackage())
				{
					var workbook = package.Workbook;
					var worksheet = workbook.Worksheets.Add("Reports");


					int index = 1;
					string cell = string.Format($"A{index}:F{index}");
					worksheet.Cells[cell].Value = "Báo cáo người dùng";
					worksheet.Cells[cell].Merge = true;
					worksheet.Cells[cell].Style.Font.Size = 20;
					worksheet.Cells[cell].Style.Font.Bold = true;
					worksheet.Cells[cell].Style.Fill.SetBackground(System.Drawing.ColorTranslator.FromHtml("#FE5303"));
					worksheet.Cells[cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
					worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					index++;

					cell = string.Format($"A{index}");
					worksheet.Cells[cell].Value = "Id";

					cell = string.Format($"B{index}");
					worksheet.Cells[cell].Value = "Người báo cáo";

					cell = string.Format($"C{index}");
					worksheet.Cells[cell].Value = "Báo cáo";

					cell = string.Format($"D{index}");
					worksheet.Cells[cell].Value = "Mô tả";

					cell = string.Format($"E{index}");
					worksheet.Cells[cell].Value = "Loại báo cáo";

					cell = string.Format($"F{index}");
					worksheet.Cells[cell].Value = "Thời gian";



					cell = string.Format($"A{index}:F{index}");
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
						worksheet.Cells[cell].Value = exportReports[i].Id;

						cell = string.Format($"B{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].FromEmail;

						cell = string.Format($"C{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].TargetEmail;

						cell = string.Format($"D{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].Description;

						cell = string.Format($"E{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].ReportType;

						cell = string.Format($"F{indexData}");
						worksheet.Cells[cell].Value = exportReports[i].ReportTime;

						worksheet.Row(indexData).Height = 30;
					}


					cell = string.Format($"A{2}:F{indexData}");
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
		string GetReportTypeString(ReportType reportType)
		{
			switch (reportType)
			{
				case ReportType.ReportStore:
					return "Báo cáo cửa hàng";
				case ReportType.ReportShipper:
					return "Báo cáo nhân viên giao hàng";
				case ReportType.ReportCustomer:
					return "Báo cáo người dùng";
				default:
					return "Unknown"; // Handle unexpected values if necessary
			}
		}


		public IEnumerable<dynamic> GetReports(ReportParameters reportParameters)
		{
			try
			{
				dynamic returnData = null;
				var parameters = new DynamicParameters();
				//parameters.Add("userId", reportParameters.uId);
				parameters.Add("usernameReport", reportParameters.UsernameReport);
				parameters.Add("description", reportParameters.Description);
				parameters.Add("pageNumber", reportParameters.PageNumber);
				parameters.Add("pageSize", reportParameters.PageSize);

				using var db = _context.Database.GetDbConnection();

				returnData = db.Query<dynamic>("GetReports", parameters, commandType: CommandType.StoredProcedure);
				db.Close();
				return returnData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		

		public List<ReportStatistic> ReportStatistics(int year)
		{
			try
			{
				var reportsByMonth = _context.Reports
	.Where(r => r.CreatedAt.Year == year ) 
	.GroupBy(r =>  r.CreatedAt.Month)
	.Select(group => new ReportStatistic
	{
		Month = group.Key,
		NumberOfReport = group.Count()
	})
	.OrderBy(group => group.Month)
	.ToList();
				return reportsByMonth;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public int CountAllReportInYear(int year)
		{
			return _context.Reports.Where(x=>x.CreatedAt.Year == year).Count();
		}
	}
}
