namespace FFS.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
