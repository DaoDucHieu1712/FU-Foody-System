using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Store")]
    public class Store : BaseEntity<int>
    {
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string AvatarURL { get;set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Combo> Combos { get; set; }
    }
}
