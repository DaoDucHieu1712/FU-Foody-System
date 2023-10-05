using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Level")]
    public class Level : BaseEntity<int>
    {
        public Rank Rank { get; set; }
        public decimal Point { get;set; }
        public ApplicationUser User { get;set; }
        
    }
}
