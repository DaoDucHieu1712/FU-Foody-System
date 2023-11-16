using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.QueryParametter
{
    public class AllStoreParameters : QueryStringParameters
    {
        public string? CategoryName { get; set; }
        //Search by Store Name
        public string? Search { get; set; }
        public FilterStore? FilterStore { get; set; }
    }
}
