using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using FFS.Application.Data;
using FFS.Application.DTOs.Report;
using FFS.Application.DTOs.Store;
using Dapper;
using System.Data;

using FFS.Application.Data;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Pqc.Crypto.Frodo;

namespace FFS.Application.Repositories.Impls {
    public class ReportRepository : EntityRepository<Report, int>, IReportRepository {
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
                List<Report> reports = await _context.Reports
                    .Where(x => x.IsDelete == false).ToListAsync();
                var exportReports = reports.Select(report => new ExportReportDTO
                {
                    Id = report.Id,
                    FromEmail = _context.Users.FirstOrDefault(x => x.Id.Equals(report.UserId)).Email,
                    TargetEmail = _context.Users.FirstOrDefault(x => x.Id.Equals(report.TargetId)).Email,
                    ReportType = GetReportTypeString(report.ReportType),
                    ReportTime = report.UpdatedAt,
                    Desciption = report.Description
                }).ToList();

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.ExportReport(exportReports, workbook);

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

    }
}
