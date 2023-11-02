namespace FFS.Application.DTOs.QueryParametter
{
    public class FoodParameters : QueryStringParameters
    {
        public int StoreId { get; set; }
        public string? FoodName { get; set; }
    }
}
