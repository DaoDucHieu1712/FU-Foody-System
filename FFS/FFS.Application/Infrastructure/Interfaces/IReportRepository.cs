using FFS.Application.Entities;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IReportRepository : IRepository<Report, int>
    {
        Task CreateReport(Report report);
        Task<byte[]> ExportReport();
    }
}
