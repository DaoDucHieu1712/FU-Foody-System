using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Chat")]
    public class Chat :BaseEntity<int>
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        [ForeignKey(nameof(FromUserId))]
        public ApplicationUser FormUser { get; set; }
        [ForeignKey(nameof(ToUserId))]
        public ApplicationUser ToUser { get; set; }
    }
}
