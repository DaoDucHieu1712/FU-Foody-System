using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Admin
{
	public class AccountStatistic
	{
		public StatusUser UserType { get; set; }
		public int? NumberOfAccount { get; set; }
	}
}
