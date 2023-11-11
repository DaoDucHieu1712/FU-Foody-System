using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Report
{
    public class ExportReportDTO
    {
        public int Id { get; set; }
        public string? FromEmail { get; set; }
        public string? TargetEmail { get; set; }
        public string? ReportType { get; set; }
        public string Desciption { get; set; }
        public DateTime ReportTime { get; set; }
    }
}
