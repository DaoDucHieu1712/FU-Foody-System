using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Message")]
    public class Message : BaseEntity<int>
    {
        public string Content { get; set; }
		public int ChatId { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
		[ForeignKey(nameof(ChatId))]
		public Chat Chat { get; set; }
    }
}
