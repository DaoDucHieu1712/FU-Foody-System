using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Wishlist")]
    public class Wishlist : BaseEntity<int>
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }   
        public ICollection<Food> Foods { get; set; }
    }
}
