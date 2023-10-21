using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Category")]
    public class Category : BaseEntity<int>
    {
        public string CategoryName { get; set; }
        public int StoreId { get;set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        public ICollection<Food> Foods { get; set;}
    }
}
