using System.Text.Json.Serialization;

namespace FFS.Application.DTOs.Chat
{
	public class MessageRequestDTO
	{
		[JsonIgnore]
		public int Id { get; set; }
		public string Content { get; set; }
		public int ChatId { get; set; }
		public string UserId { get; set; }
	}
}
