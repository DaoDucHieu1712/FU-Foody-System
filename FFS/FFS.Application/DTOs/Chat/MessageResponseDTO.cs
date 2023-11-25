namespace FFS.Application.DTOs.Chat
{
	public class MessageResponseDTO
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public int ChatId { get; set; }
		public string UserId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
