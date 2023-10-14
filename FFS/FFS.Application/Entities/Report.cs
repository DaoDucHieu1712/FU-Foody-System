using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Report")]
    public class Report : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public int ReportType { get; set; }
        public string? Description { get; set; }

    }
}
