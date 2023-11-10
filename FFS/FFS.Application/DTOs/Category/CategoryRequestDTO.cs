using System.Text.Json.Serialization;

namespace FFS.Application.DTOs.Category
{
    public class CategoryRequestDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int StoreId { get; set; }
    }
}
