using AutoMapper.Configuration.Conventions;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using static FFS.Application.Repositories.Impls.ReportRepository;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IReportRepository : IRepository<Report, int>
    {
        Task CreateReport(Report report);
        Task<byte[]> ExportReport();
        IEnumerable<dynamic> GetReports(ReportParameters reportParameters);
        int CountGetReports(ReportParameters reportParameters);
		int CountAllReportInYear(int year);
		List<ReportStatistic> ReportStatistics(int year);
		//Task<List<ReportTypeCount>> GetTotalReportsByTypeAsync();
		Task<List<ReportTypeCount>> GetReportsPerMonth();

	}
}
