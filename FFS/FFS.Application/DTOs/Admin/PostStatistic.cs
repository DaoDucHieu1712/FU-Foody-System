using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Admin
{
	public class PostStatistic
	{
		public StatusPost PostStatus { get; set; }
		public int NumberOfPosts { get; set; }
	}
}
