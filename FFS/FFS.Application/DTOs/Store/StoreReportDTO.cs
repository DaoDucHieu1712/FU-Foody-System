using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Store
{
    public class StoreReportDTO
    {
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public string? Description { get; set; }
    }
}
