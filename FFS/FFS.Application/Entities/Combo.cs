using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
	[Table("Combo")]
	public class Combo : BaseEntity<int>
	{

		public Combo()
		{
			OrderDetails = new HashSet<OrderDetail>();
		}

		public string Name { get; set; }
		public int StoreId { get; set; }
		[ForeignKey(nameof(StoreId))]
		public Store Store { get; set; }
		public int Percent { get; set; }
		public string Image { get; set; }
		public ICollection<OrderDetail> OrderDetails { get; set; }
	}
}
