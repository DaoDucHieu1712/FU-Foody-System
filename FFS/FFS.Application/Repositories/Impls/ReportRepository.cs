using System.Data;
using System.Globalization;
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
				var monthNames = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

				var reportsByMonth = Enumerable.Range(1, 12)
					.Select(month => new ReportStatistic
					{
						Month = monthNames[month - 1],
						NumberOfReport = _context.Reports
							.Count(r => r.CreatedAt.Year == year && r.CreatedAt.Month == month)
					})
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

		public class ReportTypeCount
		{
			public int Year { get; set; }
			public int Month { get; set; }
			public ReportType ReportType { get; set; }
			public int TotalReports { get; set; }
		}
		//public async Task<List<ReportTypeCount>> GetTotalReportsByTypeAsync()
		//{
		//	DateTime today = DateTime.Now;
		//	DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
		//	DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

		//	var result = await _context.Reports
		//		.Where(r => r.CreatedAt >= firstDayOfMonth && r.CreatedAt <= lastDayOfMonth)
		//		.GroupBy(r => r.ReportType)
		//		.Select(g => new ReportTypeCount
		//		{
		//			ReportType = g.Key,
		//			TotalReports = g.Count()
		//		})
		//		.ToListAsync();
		//	//	var reportsPerMonth = _context.Reports
		//	//.GroupBy(report => new { Year = report.CreatedAt.Year, Month = report.CreatedAt.Month, report.ReportType })
		//	//.Select(group => new
		//	//{
		//	//	Year = group.Key.Year,
		//	//	Month = group.Key.Month,
		//	//	ReportType = group.Key.ReportType,
		//	//	TotalReports = group.Count()
		//	//})
		//	//.OrderBy(group => group.Year)
		//	//.ThenBy(group => group.Month)
		//	//.ThenBy(group => group.ReportType)
		//	//.ToList();

		//	return result;
		//}
		public async Task<List<ReportTypeCount>> GetReportsPerMonth()
		{
			var reportsPerMonth = _context.Reports
				.GroupBy(report => new { Year = report.CreatedAt.Year, Month = report.CreatedAt.Month, report.ReportType })
				.Select(group => new ReportTypeCount
				{
					Year = group.Key.Year,
					Month = group.Key.Month,
					ReportType = group.Key.ReportType,
					TotalReports = group.Count()
				})
				.OrderBy(group => group.Year)
				.ThenBy(group => group.Month)
				.ThenBy(group => group.ReportType)
				.ToList();

			return reportsPerMonth;
		}


	}
}
