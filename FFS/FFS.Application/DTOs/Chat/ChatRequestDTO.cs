using System.Text.Json.Serialization;

namespace FFS.Application.DTOs.Chat
{
	public class ChatRequestDTO
	{
		[JsonIgnore]
		public int Id { get; set; }
		public string FromUserId { get; set; }
		public string ToUserId { get; set; }
	}
}
