namespace FFS.Application.DTOs.QueryParametter
{
    public class PostParameters: QueryStringParameters
    {
        public string? PostTitle { get; set; }
        public string? OrderBy { get; set; }

    }
}
