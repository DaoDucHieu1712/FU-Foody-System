using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity<int>
    {
        public string Content { get; set; }
        public int? Rate { get; set; }
        public string? UserId { get; set; }
        public int? StoreId { get; set; }
        public int? FoodId { get; set; }
        public int ParentCommentId { get; set; }
        [ForeignKey(nameof(ParentCommentId))]
        public Comment? ParentComment { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food? Food { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<React> Reacts { get; set; }
        public virtual ICollection<Comment> ParentComments { get; set; }
    }
}
