using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IReportRepository : IRepository<Report, int>
    {
        Task CreateReport(Report report);
        Task<byte[]> ExportReport();
        IEnumerable<dynamic> GetReports(ReportParameters reportParameters);
        int CountGetReports(ReportParameters reportParameters);
    }
}
