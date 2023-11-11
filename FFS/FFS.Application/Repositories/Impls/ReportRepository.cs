using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using FFS.Application.Data;
using FFS.Application.DTOs.Report;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class ReportRepository : EntityRepository<Report, int>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context) : base(context)
        {
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
    }
}
