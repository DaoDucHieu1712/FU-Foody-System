using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("React")]
    public class React : BaseEntity<int>
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public int? CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
        public bool IsLike { get; set; }
    }
}
