using FFS.Application.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.Food
{
    public class FoodDTO
    {
        public virtual int Id { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string? FoodName { get; set; }
        public string? ImageURL { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? StoreId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
