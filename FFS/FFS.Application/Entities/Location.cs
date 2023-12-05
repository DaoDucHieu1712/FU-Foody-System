using System.ComponentModel.DataAnnotations.Schema;
using FFS.Application.Entities.Common;

namespace FFS.Application.Entities
{
	[Table("Location")]
	public class Location : BaseEntity<int>
	{
		public string UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public ApplicationUser? User { get; set; }
		public int ProvinceID { get; set; }
		public string ProvinceName { get; set; }
		public int DistrictID { get; set; }
		public string DistrictName { get; set; }
		public string WardCode { get; set; }
		public string WardName { get; set; }
		public string Address { get; set; }
		public bool IsDefault { get; set; }
		public string? Description { get; set; }
		public string? Receiver { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
