using System.ComponentModel.DataAnnotations.Schema;
using FFS.Application.Entities.Common;

namespace FFS.Application.Entities
{
	[Table("UserDiscount")]
	public class UserDiscount : BaseEntity<int>
	{
		public string UserId { get; set; }
		public int DiscountId { get; set; }
		[ForeignKey(nameof(UserId))]
		public ApplicationUser User { get; set; }
		[ForeignKey(nameof(DiscountId))]
		public Discount Discount { get; set; }
	}
}
