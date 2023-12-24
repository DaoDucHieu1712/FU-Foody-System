using FFS.Application.Entities.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Notification")]
    public class Notification : BaseEntity<int>
    {
        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

		public bool IsRead { get; set; }
	}
}
