using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    public class ReactPost : BaseEntity<int>
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public int? PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
        public bool IsLike { get; set; }
    }
}
