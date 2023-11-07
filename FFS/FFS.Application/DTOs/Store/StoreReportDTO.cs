using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Store
{
    public class ReportDTO
    {
        public string UserId { get; set; }
        public string TargetId { get; set; }
        public ReportType ReportType { get; set; }
        public string? Description { get; set; }
    }
}
