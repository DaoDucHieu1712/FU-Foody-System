﻿using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Post")]
    public class Post : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}