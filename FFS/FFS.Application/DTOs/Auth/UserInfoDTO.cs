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
		public string? PhoneNumber { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }	
		public bool? Gender { get; set; }
		public DateTime BirthDay { get; set; }
		public string? Email { get; set; }
		public  List<PostDTO> Posts { get; set; }
		public int? TotalPost { get; set; }
		public int? TotalRecentComments{ get; set; }
	}
}
