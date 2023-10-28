using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Report")]
    public class Report : BaseEntity<int>
    {
        public string UserId { get; set; }
        public string TargetId { get; set; }
        public ReportType ReportType { get; set; }
        public string? Description { get; set; }

    }
}
