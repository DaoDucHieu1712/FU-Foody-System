using System.ComponentModel.DataAnnotations;

namespace FFS.Application.Entities.Common
{
    public class BaseEntity<T>
    {
        [Key]
        public virtual T Id { get;set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual bool IsDelete { get; set; }
    }
}
