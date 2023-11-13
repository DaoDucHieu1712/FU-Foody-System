namespace FFS.Application.DTOs.QueryParametter
{
    public class CategoryParameters: QueryStringParameters
    {
        public int StoreId { get; set; }
        public string? CategoryName { get; set; }
    }
}
