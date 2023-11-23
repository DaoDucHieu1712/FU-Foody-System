using FFS.Application.DTOs.Post;
using FFS.Application.Entities.Enum;
using FFS.Application.Entities;

namespace FFS.Application.DTOs.Auth
{
	public class UserInfoDTO
	{
		public string? Id { get; set; }
		public string? Avatar { get; set; }
		public string? UserName { get; set; }
		public  List<PostDTO> Posts { get; set; }
		public int? TotalPost { get; set; }
		public int? TotalRecentComments{ get; set; }
	}
}
