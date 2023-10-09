using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Location")]
    public class Location : BaseEntity<int>
    {
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
        public string? Address { get; set; }
        public bool? IsDefault { get; set; }
    }
}
