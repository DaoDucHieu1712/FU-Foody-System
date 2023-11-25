namespace FFS.Application.DTOs.Chat
{
	public class ChatResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string FromUserName { get; set; }
		public string FromUserId { get; set; }
		public string FromUserImage { get; set; }
		public string ToUserName { get; set; }
		public string ToUserId { get; set; }
		public string ToUserImage { get; set; }
		public List<MessageResponseDTO> Messages { get; set; }
	}
}
