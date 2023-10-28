using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

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
    }
}
